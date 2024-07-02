using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetProfileAsync(int userId);
        Task<bool> SignUpAsync(SignUpDTO signUpData);
        Task<UserType?> SignInAsync(SignInDTO signInData);
        Task<bool> UpdateProfileAsync(UserDTO updateData);
        Task<bool> DeleteUserProfileAsync(int userId, UserType userType);
        Task<UserDTO> GetUserByEmailAsync(string email, UserType userType);
        Task<Result> ChangePasswordAsync(ChangePasswordDTO changePasswordData);
        Task<bool> VerifyOldPasswordAsync(ChangePasswordDTO changePasswordData);
        Task<(object user, string userType)> AuthenticateAsync(string email, string password);
    }
}

