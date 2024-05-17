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
    }
}

