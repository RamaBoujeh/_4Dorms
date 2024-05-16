    using _4Dorms.Repositories.implementation;
    using _4Dorms.Repositories.Interfaces;
    using _4Dorms.Resources;
    using Microsoft.AspNetCore.Mvc;

    namespace _4Dorms.Controllers
    {
        [ApiController]
        [Route("api/[Controller]")]
        public class UserController : ControllerBase
        {
            private readonly IUserService _userService;
            private readonly Dictionary<string, string> _verificationCodes;
    

            public UserController(IUserService userService)
            {
                _userService = userService;
                _verificationCodes = new Dictionary<string, string>();
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
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userType = await _userService.SignInAsync(signInData);

            if (userType.HasValue)
            {
                // Determine the appropriate redirection based on the user type
                switch (userType)
                {
                    case UserType.Student:
                        // Redirect to the student page
                        return Ok("User signed in successfully as a student.");

                    case UserType.DormitoryOwner:
                        // Redirect to the dormitory owner page
                        return Ok("User signed in successfully as a dormitory owner.");

                    case UserType.Administrator:
                        // Redirect to the administrator page
                        return Ok("User signed in successfully as an administrator.");

                    default:
                        // Unexpected user type (shouldn't happen)
                        return BadRequest("Invalid user type.");
                }
            }
            else
            {
                return Unauthorized("Invalid email or password.");
            }
        }

        [HttpGet("check-signed-in")]
        public async Task<IActionResult> CheckSignedIn()
        {
            try
            {
                var result = await _userService.CheckSignedInAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
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

    

