using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using System.Runtime.CompilerServices;

namespace _4Dorms.Repositories.implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<DormitoryOwner> _dormitoryOwnerRepository;
        private readonly IGenericRepository<Administrator> _administratorRepository;
        private readonly IGenericRepository<FavoriteList> _favoriteListRepository;

        public UserService(IGenericRepository<Student> studentRepository, IGenericRepository<DormitoryOwner> dormitoryOwnerRepository,
            IGenericRepository<Administrator> administratorRepository, IGenericRepository<FavoriteList> favoriteListRepository)
        {
            _administratorRepository = administratorRepository;
            _studentRepository = studentRepository;
            _dormitoryOwnerRepository = dormitoryOwnerRepository;
            _favoriteListRepository = favoriteListRepository;
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
                        if (signUpData.CreateFavoriteList)
                        {
                            await CreateEmptyFavoriteListForUser(student.StudentId, UserType.Student);
                        }
                        break;

                    case UserType.DormitoryOwner:
                        var dormitoryOwner = MapToDormitoryOwner(signUpData);
                        _dormitoryOwnerRepository.Add(dormitoryOwner);
                        await _dormitoryOwnerRepository.SaveChangesAsync();
                        if (signUpData.CreateFavoriteList)
                        {
                            await CreateEmptyFavoriteListForUser(dormitoryOwner.DormitoryOwnerId, UserType.DormitoryOwner);
                        }
                        break;

                    default:
                        return false; 
                }

               // await _administratorRepository.SaveChangesAsync();

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
                Disabilities = signUpData.Disabilities
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
                DateOfBirth = signUpData.DateOfBirth
            };
        }

       

        public async Task<bool> SignInAsync(SignInDTO signInData)
        {
            switch (signInData.UserType)
            {
                case UserType.Student:
                    var student = await _studentRepository.FindByConditionAsync(s => s.Email == signInData.Email && s.Password == signInData.Password);
                    return student != null;

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.FindByConditionAsync(d => d.Email == signInData.Email && d.Password == signInData.Password);
                    return dormitoryOwner != null;

                case UserType.Administrator:
                    var administrator = await _administratorRepository.FindByConditionAsync(a => a.Email == signInData.Email && a.Password == signInData.Password);
                    return administrator != null;

                default:
                    return false;
            }
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
        }

        private void MapToDormitoryOwner(UserDTO updateData, DormitoryOwner dormitoryOwner)
        {
            dormitoryOwner.Name = updateData.Name;
            dormitoryOwner.Email = updateData.Email;
            dormitoryOwner.Password = updateData.Password;
            dormitoryOwner.Gender = updateData.Gender;
            dormitoryOwner.PhoneNumber = updateData.PhoneNumber;
            dormitoryOwner.DateOfBirth = updateData.DateOfBirth;
        }

        private void MapToAdministrator(UserDTO updateData, Administrator administrator)
        {
            administrator.Name = updateData.Name;
            administrator.PhoneNumber = updateData.PhoneNumber;
            administrator.Email = updateData.Email;
            administrator.Password = updateData.Password;
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
        public async Task<bool> DeleteUserProfileAsync(int userId, UserType userType)  
        {
            switch (userType)
            {
                case UserType.Student:
                    var student = await _studentRepository.GetByIdAsync(userId);
                    if (student == null)
                        return false;
                    _studentRepository.Remove(userId);
                    return await _studentRepository.SaveChangesAsync() ;

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.GetByIdAsync(userId);
                    if (dormitoryOwner == null)
                        return false;
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
    }
}
