using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using System;
using System.Threading.Tasks;

namespace _4Dorms.Repositories.implementation
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<DormitoryOwner> _dormitoryOwnerRepository;
        private readonly IGenericRepository<Administrator> _administratorRepository;
        private readonly IGenericRepository<FavoriteList> _favoriteListRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private bool _isUserSignedIn = false;

        public UserService(IGenericRepository<Student> studentRepository, IGenericRepository<DormitoryOwner> dormitoryOwnerRepository,
            IGenericRepository<Administrator> administratorRepository, IGenericRepository<FavoriteList> favoriteListRepository, IHttpContextAccessor httpContextAccessor)
        {
            _administratorRepository = administratorRepository;
            _studentRepository = studentRepository;
            _dormitoryOwnerRepository = dormitoryOwnerRepository;
            _favoriteListRepository = favoriteListRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> SignUpAsync(SignUpDTO signUpData)
        {
            try
            {
                switch (signUpData.UserType)
                {
                    case UserType.Student:
                        var student = MapToStudent(signUpData);
                        _studentRepository.Add(student);
                        await _studentRepository.SaveChangesAsync();
                        await CreateEmptyFavoriteListForUser(student.StudentId, UserType.Student);
                        break;

                    case UserType.DormitoryOwner:
                        var dormitoryOwner = MapToDormitoryOwner(signUpData);
                        _dormitoryOwnerRepository.Add(dormitoryOwner);
                        await _dormitoryOwnerRepository.SaveChangesAsync();
                        await CreateEmptyFavoriteListForUser(dormitoryOwner.DormitoryOwnerId, UserType.DormitoryOwner);
                        break;

                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during user sign-up: {ex.Message}");
                return false;
            }
        }

        private async Task CreateEmptyFavoriteListForUser(int userId, UserType userType)
        {
            var favoriteList = new FavoriteList();

            switch (userType)
            {
                case UserType.Student:
                    favoriteList.StudentId = userId;
                    break;

                case UserType.DormitoryOwner:
                    favoriteList.DormitoryOwnerId = userId;
                    break;

                default:
                    return;
            }

            _favoriteListRepository.Add(favoriteList);
            await _favoriteListRepository.SaveChangesAsync();
        }
        private Student MapToStudent(SignUpDTO signUpData)
        {
            return new Student
            {
                Name = signUpData.Name,
                Email = signUpData.Email,
                Password = signUpData.Password,
                PhoneNumber = signUpData.PhoneNumber,
                Gender = signUpData.Gender,
                DateOfBirth = signUpData.DateOfBirth,
                Disabilities = signUpData.Disabilities,
                ProfilePictureUrl = signUpData.ProfilePictureUrl
            };
        }

        private DormitoryOwner MapToDormitoryOwner(SignUpDTO signUpData)
        {
            return new DormitoryOwner
            {
                Name = signUpData.Name,
                Email = signUpData.Email,
                Password = signUpData.Password,
                PhoneNumber = signUpData.PhoneNumber,
                Gender = signUpData.Gender,
                DateOfBirth = signUpData.DateOfBirth,
                ProfilePictureUrl = signUpData.ProfilePictureUrl
            };
        }

        public async Task<UserType?> SignInAsync(SignInDTO signInData)
        {
            var student = await _studentRepository.FindByConditionAsync(s => s.Email == signInData.Email && s.Password == signInData.Password);
            if (student != null)
            {
                _isUserSignedIn = true;
                return UserType.Student;
            }

            var dormitoryOwner = await _dormitoryOwnerRepository.FindByConditionAsync(d => d.Email == signInData.Email && d.Password == signInData.Password);
            if (dormitoryOwner != null)
            {
                _isUserSignedIn = true;
                return UserType.DormitoryOwner;
            }

            var administrator = await _administratorRepository.FindByConditionAsync(a => a.Email == signInData.Email && a.Password == signInData.Password);
            if (administrator != null)
            {
                _isUserSignedIn = true;
                return UserType.Administrator;
            }

            // If no user is found, return null or an appropriate value to indicate failure.
            return null;
        }


        public async Task<bool> UpdateProfileAsync(UserDTO updateData)
        {
            switch (updateData.UserType)
            {
                case UserType.Student:
                    var student = await _studentRepository.GetByIdAsync(updateData.UserId);
                    if (student == null)
                        return false;

                    MapToStudent(updateData, student);
                    _studentRepository.Update(student);
                    break;

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.GetByIdAsync(updateData.UserId);
                    if (dormitoryOwner == null)
                        return false;

                    MapToDormitoryOwner(updateData, dormitoryOwner);
                    _dormitoryOwnerRepository.Update(dormitoryOwner);
                    break;

                case UserType.Administrator:
                    var administrator = await _administratorRepository.GetByIdAsync(updateData.UserId);
                    if (administrator == null)
                        return false;

                    MapToAdministrator(updateData, administrator);
                    _administratorRepository.Update(administrator);
                    break;

                default:
                    return false;
            }

            return await SaveChangesAsync(updateData.UserType);
        }

        private void MapToStudent(UserDTO updateData, Student student)
        {
            student.Name = updateData.Name;
            student.Email = updateData.Email;
            student.Password = updateData.Password;
            student.Gender = updateData.Gender;
            student.PhoneNumber = updateData.PhoneNumber;
            student.DateOfBirth = updateData.DateOfBirth;
            student.Disabilities = updateData.Disabilities;
            student.ProfilePictureUrl = updateData.ProfilePictureUrl;
        }

        private void MapToDormitoryOwner(UserDTO updateData, DormitoryOwner dormitoryOwner)
        {
            dormitoryOwner.Name = updateData.Name;
            dormitoryOwner.Email = updateData.Email;
            dormitoryOwner.Password = updateData.Password;
            dormitoryOwner.Gender = updateData.Gender;
            dormitoryOwner.PhoneNumber = updateData.PhoneNumber;
            dormitoryOwner.DateOfBirth = updateData.DateOfBirth;
            dormitoryOwner.ProfilePictureUrl = updateData.ProfilePictureUrl;
        }

        private void MapToAdministrator(UserDTO updateData, Administrator administrator)
        {
            administrator.Name = updateData.Name;
            administrator.PhoneNumber = updateData.PhoneNumber;
            administrator.Email = updateData.Email;
            administrator.Password = updateData.Password;
            administrator.ProfilePictureUrl = updateData.ProfilePictureUrl;
        }

        private async Task<bool> SaveChangesAsync(UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    return await _studentRepository.SaveChangesAsync();
                case UserType.DormitoryOwner:
                    return await _dormitoryOwnerRepository.SaveChangesAsync();
                case UserType.Administrator:
                    return await _administratorRepository.SaveChangesAsync();
                default:
                    return false;
            }
        }

        public void SignOut()
        {
            _isUserSignedIn = false;
        }

        public bool IsUserSignedIn()
        {
            return _isUserSignedIn;
        }

        public async Task<bool> DeleteUserProfileAsync(int userId, UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    var student = await _studentRepository.GetByIdAsync(userId);
                    if (student == null)
                        return false;

                    await RemoveFavoriteListForUser(userId, UserType.Student);

                    _studentRepository.Remove(userId);
                    return await _studentRepository.SaveChangesAsync();

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.GetByIdAsync(userId);
                    if (dormitoryOwner == null)
                        return false;

                    await RemoveFavoriteListForUser(userId, UserType.DormitoryOwner);

                    _dormitoryOwnerRepository.Remove(userId);
                    return await _dormitoryOwnerRepository.SaveChangesAsync();

                case UserType.Administrator:
                    var administrator = await _administratorRepository.GetByIdAsync(userId);
                    if (administrator == null)
                        return false;


                    _administratorRepository.Remove(userId);
                    return await _administratorRepository.SaveChangesAsync();

                default:
                    throw new ArgumentException("Invalid user type.");
            }
        }




        private async Task RemoveFavoriteListForUser(int favoriteListId, UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    var studentFavoriteList = await _favoriteListRepository.GetByIdAsync(favoriteListId);
                    if (studentFavoriteList != null)
                    {
                        _favoriteListRepository.Remove(favoriteListId);
                        await _favoriteListRepository.SaveChangesAsync();
                    }
                    break;

                case UserType.DormitoryOwner:
                    var dormitoryOwnerFavoriteList = await _favoriteListRepository.GetByIdAsync(favoriteListId);
                    if (dormitoryOwnerFavoriteList != null)
                    {
                        _favoriteListRepository.Remove(favoriteListId);
                        await _favoriteListRepository.SaveChangesAsync();
                    }
                    break;

                default:
                    break;
            }
        }


    }
}

