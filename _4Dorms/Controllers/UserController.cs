    using _4Dorms.Repositories.implementation;
    using _4Dorms.Repositories.Interfaces;
    using _4Dorms.Resources;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using _4Dorms.Models;
using System.Security.Cryptography;

namespace _4Dorms.Controllers
    {
    [ApiController] 
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Dictionary<string, string> _verificationCodes;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _verificationCodes = new Dictionary<string, string>();
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.SignUpAsync(signUpData);
                if (result)
                {
                    return Ok("User signed up successfully.");
                }
                else
                {
                    return BadRequest("Failed to sign up user. The email is already in use.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during sign-up: {ex.Message}");
                return StatusCode(500, "An error occurred during sign-up.");
            }
        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO model)
        {
            var (user, userType) = await _userService.AuthenticateAsync(model.Email, model.Password);
            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("Rama-Issam-Boujeh-backend-project");

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, model.Email),
            new Claim(ClaimTypes.Role, userType)
        };

                // Add specific claims based on user type
                switch (user)
                {
                    case Student student:
                        claims.Add(new Claim("StudentId", student.StudentId.ToString()));
                        claims.Add(new Claim("Name", student.Name));
                        claims.Add(new Claim("PhoneNumber", student.PhoneNumber));
                        claims.Add(new Claim("Gender", student.Gender));
                        claims.Add(new Claim("DateOfBirth", student.DateOfBirth.ToString("o")));
                        claims.Add(new Claim("Disabilities", student.Disabilities ?? string.Empty));
                        claims.Add(new Claim("ProfilePictureUrl", student.ProfilePictureUrl ?? string.Empty));
                        break;

                    case DormitoryOwner dormitoryOwner:
                        claims.Add(new Claim("DormitoryOwnerId", dormitoryOwner.DormitoryOwnerId.ToString()));
                        claims.Add(new Claim("Name", dormitoryOwner.Name));
                        claims.Add(new Claim("PhoneNumber", dormitoryOwner.PhoneNumber));
                        claims.Add(new Claim("Gender", dormitoryOwner.Gender));
                        claims.Add(new Claim("DateOfBirth", dormitoryOwner.DateOfBirth.ToString("o")));
                        claims.Add(new Claim("ProfilePictureUrl", dormitoryOwner.ProfilePictureUrl ?? string.Empty));
                        break;

                    case Administrator administrator:
                        claims.Add(new Claim("AdministratorId", administrator.AdministratorId.ToString()));
                        claims.Add(new Claim("Name", administrator.Name));
                        claims.Add(new Claim("PhoneNumber", administrator.PhoneNumber));
                        claims.Add(new Claim("ProfilePictureUrl", administrator.ProfilePictureUrl ?? string.Empty));
                        break;
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(400),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "YourIssuer",
                    Audience = "YourAudience"
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }


        [HttpGet("check-signed-in")]
        [Authorize]
        public IActionResult CheckSignedIn()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new { signedIn = true });
            }
            else
            {
                return Ok(new { signedIn = false });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var userProfile = await _userService.GetProfileAsync(int.Parse(userId));
            if (userProfile == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(userProfile);
        }



        [HttpPost("send-verification-code")]
            public async Task<IActionResult> SendVerificationCode([FromBody] ForgotPasswordRequest request)
            {
                string verificationCode = GenerateVerificationCode();

                _verificationCodes[request.PhoneNumber] = verificationCode;

                return Ok(new { VerificationCode = verificationCode });
            }

            [HttpPost("verify-code")]
            public IActionResult VerifyCode([FromBody] VerifyCodeRequest request)
            {
                // Check if the received verification code matches the stored code
                if (_verificationCodes.TryGetValue(request.PhoneNumber, out string storedCode) && storedCode == request.VerificationCode)
                {
                    // Verification successful
                    // Clear the stored verification code
                    _verificationCodes.Remove(request.PhoneNumber);
                    return Ok("Verification successful.");
                }
                else
                {
                    return BadRequest("Invalid verification code.");
                }
            }

            private string GenerateVerificationCode()
            {
                Random random = new Random();
                return random.Next(100000, 999999).ToString();
            }

        [HttpPost("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO updateData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = await _userService.UpdateProfileAsync(updateData);
            if (success)
            {
                return Ok("Profile updated successfully.");
            }
            else
            {
                return BadRequest("Failed to update profile.");
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || int.Parse(userId) != changePasswordData.UserId)
            {
                return Unauthorized();
            }

            var result = await _userService.ChangePasswordAsync(changePasswordData);
            if (result.IsSuccess)
            {
                return Ok(new { message = "Password changed successfully." });
            }
            else
            {
                return BadRequest(new { message = result.Message });
            }
        }
        [HttpPost("verify-old-password")]
        [Authorize]
        public async Task<IActionResult> VerifyOldPassword([FromBody] ChangePasswordDTO changePasswordData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || int.Parse(userId) != changePasswordData.UserId)
            {
                return Unauthorized();
            }

            bool isValid = await _userService.VerifyOldPasswordAsync(changePasswordData);
            if (isValid)
            {
                return Ok(new { message = "Old password verified successfully." });
            }
            else
            {
                return BadRequest(new { message = "Old password is incorrect." });
            }
        }

        [HttpDelete("{userId}/{userType}")]
        public async Task<IActionResult> DeleteUser(int userId, UserType userType)
        {
            var result = await _userService.DeleteUserProfileAsync(userId, userType);

            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
} 

    

