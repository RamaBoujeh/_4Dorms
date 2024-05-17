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
                    return StatusCode(500, "Failed to sign up user.");
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
            // Validate user credentials by checking against the database
            var userType = await _userService.SignInAsync(model);
            if (userType != null)
            {
                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();

                // Generate key
                var key = Encoding.ASCII.GetBytes("Rama-Issam-Boujeh-backend-project");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, userType.ToString()) // Add user role as a claim
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "YourIssuer",
                    Audience = "YourAudience"
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Return the token
                return Ok(new { token = tokenString }); // Ensure the key matches what you're checking in the frontend
            }

            // If no user is found or credentials are incorrect, return unauthorized
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

    

