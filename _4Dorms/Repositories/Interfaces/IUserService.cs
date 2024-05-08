using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IUserService
    {
        Task<bool> SignUpAsync(SignUpDTO signUpData);
        Task<bool> SignInAsync(SignInDTO signInData);
        Task<bool> UpdateProfileAsync(UserDTO updateData);
        Task<bool> DeleteUserProfileAsync(int userId, UserType userType);
    }
}

