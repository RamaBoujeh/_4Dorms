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
        private readonly IGenericRepository<Dormitory> _dormitoryRepository;
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly IGenericRepository<DormitoryImage> _dormitoryImageRepository;
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;

        public UserService(IGenericRepository<Student> studentRepository, IGenericRepository<DormitoryOwner> dormitoryOwnerRepository,
            IGenericRepository<Administrator> administratorRepository, IGenericRepository<FavoriteList> favoriteListRepository,
            IHttpContextAccessor httpContextAccessor, IGenericRepository<Dormitory> dormitoryRepository, IGenericRepository<Room> roomRepository,
            IGenericRepository<DormitoryImage> dormitoryImageRepository, IGenericRepository<Review> reviewRepository, IGenericRepository<Booking> bookingRepository)
        {
            _administratorRepository = administratorRepository;
            _studentRepository = studentRepository;
            _dormitoryOwnerRepository = dormitoryOwnerRepository;
            _favoriteListRepository = favoriteListRepository;
            _httpContextAccessor = httpContextAccessor;
            _dormitoryRepository = dormitoryRepository;
            _roomRepository = roomRepository;
            _dormitoryImageRepository = dormitoryImageRepository;
            _reviewRepository = reviewRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> SignUpAsync(SignUpDTO signUpData)
        {
            try
            {
                // Check if the email already exists in the database
                if (await IsEmailExistsAsync(signUpData.Email))
                {
                    return false; // Email already exists, return false
                }

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

        private async Task<bool> IsEmailExistsAsync(string email)
        {
            // Check if the email exists in any of the user tables (Student, DormitoryOwner, Administrator)
            return await _studentRepository.AnyAsync(s => s.Email == email) ||
                   await _dormitoryOwnerRepository.AnyAsync(d => d.Email == email) ||
                   await _administratorRepository.AnyAsync(a => a.Email == email);
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
                return UserType.Student;
            }

            var dormitoryOwner = await _dormitoryOwnerRepository.FindByConditionAsync(d => d.Email == signInData.Email && d.Password == signInData.Password);
            if (dormitoryOwner != null)
            {
                return UserType.DormitoryOwner;
            }

            var administrator = await _administratorRepository.FindByConditionAsync(a => a.Email == signInData.Email && a.Password == signInData.Password);
            if (administrator != null)
            {
                return UserType.Administrator;
            }
            return null;
        }

        public async Task<(object user, string userType)> AuthenticateAsync(string email, string password)
        {
            var student = await _studentRepository.StudentGetByEmailAndPasswordAsync(email, password);
            if (student != null)
            {
                return (student, "Student");
            }

            var dormitoryOwner = await _dormitoryOwnerRepository.DormOwnerGetByEmailAndPasswordAsync(email, password);
            if (dormitoryOwner != null)
            {
                return (dormitoryOwner, "DormitoryOwner");
            }

            var administrator = await _administratorRepository.AdminGetByEmailAndPasswordAsync(email, password);
            if (administrator != null)
            {
                return (administrator, "Administrator");
            }

            return (null, null);
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

        public async Task<bool> DeleteUserProfileAsync(int userId, UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    var student = await _studentRepository.GetByIdAsync(userId);
                    if (student == null)
                        return false;

                    await RemoveFavoriteListsForUser(userId, UserType.Student);
                    await RemoveReviewsForStudent(userId);
                    await RemoveBookingsForStudent(userId);

                    _studentRepository.Remove(userId);
                    return await _studentRepository.SaveChangesAsync();

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.GetByIdAsync(userId);
                    if (dormitoryOwner == null)
                        return false;

                    await RemoveDormitoriesForOwner(userId);
                    await RemoveFavoriteListsForUser(userId, UserType.DormitoryOwner);

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

        private async Task RemoveReviewsForStudent(int studentId)
        {
            var reviews = _reviewRepository.Query().Where(r => r.StudentId == studentId).ToList();
            foreach (var review in reviews)
            {
                _reviewRepository.Remove(review.ReviewId);
            }
            await _reviewRepository.SaveChangesAsync();
        }

        private async Task RemoveBookingsForStudent(int studentId)
        {
            var bookings = _bookingRepository.Query().Where(b => b.StudentId == studentId).ToList();
            foreach (var booking in bookings)
            {
                _bookingRepository.Remove(booking.BookingId);
            }
            await _bookingRepository.SaveChangesAsync();
        }

        private async Task RemoveDormitoriesForOwner(int ownerId)
        {
            var dormitories = _dormitoryRepository.Query().Where(d => d.DormitoryOwnerId == ownerId).ToList();

            foreach (var dormitory in dormitories)
            {
                await RemoveDormitoryRelatedEntities(dormitory.DormitoryId);
                _dormitoryRepository.Remove(dormitory.DormitoryId);
            }

            await _dormitoryRepository.SaveChangesAsync();
        }

        private async Task RemoveDormitoryRelatedEntities(int dormitoryId)
        {
            // Remove Rooms
            var rooms = _roomRepository.Query().Where(r => r.DormitoryId == dormitoryId).ToList();
            foreach (var room in rooms)
            {
                _roomRepository.Remove(room.RoomID);
            }

            // Remove DormitoryImages
            var images = _dormitoryImageRepository.Query().Where(img => img.DormitoryId == dormitoryId).ToList();
            foreach (var image in images)
            {
                _dormitoryImageRepository.Remove(image.ImageId);
            }

            // Remove Reviews
            var reviews = _reviewRepository.Query().Where(rv => rv.DormitoryId == dormitoryId).ToList();
            foreach (var review in reviews)
            {
                _reviewRepository.Remove(review.ReviewId);
            }

            // Remove from FavoriteLists
            var favoriteLists = _favoriteListRepository.Query().Where(fl => fl.Dormitories.Any(d => d.DormitoryId == dormitoryId)).ToList();
            foreach (var favoriteList in favoriteLists)
            {
                var dormitory = favoriteList.Dormitories.FirstOrDefault(d => d.DormitoryId == dormitoryId);
                if (dormitory != null)
                {
                    favoriteList.Dormitories.Remove(dormitory);
                    _favoriteListRepository.Update(favoriteList);
                }
            }

            await _roomRepository.SaveChangesAsync();
            await _dormitoryImageRepository.SaveChangesAsync();
            await _reviewRepository.SaveChangesAsync();
            await _favoriteListRepository.SaveChangesAsync();
        }


        private async Task RemoveFavoriteListsForUser(int userId, UserType userType)
        {
            var favoriteLists = _favoriteListRepository.Query().Where(fl =>
                (userType == UserType.Student && fl.StudentId == userId) ||
                (userType == UserType.DormitoryOwner && fl.DormitoryOwnerId == userId)).ToList();

            foreach (var favoriteList in favoriteLists)
            {
                _favoriteListRepository.Remove(favoriteList.FavoriteId);
            }

            await _favoriteListRepository.SaveChangesAsync();
        }
        //-------------------------------------------------------------------------------------
        public async Task<UserDTO> GetProfileAsync(int userId)
        {
            var student = await _studentRepository.GetByIdAsync(userId);
            if (student != null)
            {
                return MapToUserDTO(student);
            }

            var dormitoryOwner = await _dormitoryOwnerRepository.GetByIdAsync(userId);
            if (dormitoryOwner != null)
            {
                return MapToUserDTO(dormitoryOwner);
            }

            var administrator = await _administratorRepository.GetByIdAsync(userId);
            if (administrator != null)
            {
                return MapToUserDTO(administrator);
            }

            return null;
        }

        private UserDTO MapToUserDTO(Student student)
        {
            return new UserDTO
            {
                UserId = student.StudentId,
                Name = student.Name,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                Disabilities = student.Disabilities,
                ProfilePictureUrl = student.ProfilePictureUrl,
                UserType = UserType.Student
            };
        }

        private UserDTO MapToUserDTO(DormitoryOwner dormitoryOwner)
        {
            return new UserDTO
            {
                UserId = dormitoryOwner.DormitoryOwnerId,
                Name = dormitoryOwner.Name,
                Email = dormitoryOwner.Email,
                PhoneNumber = dormitoryOwner.PhoneNumber,
                Gender = dormitoryOwner.Gender,
                DateOfBirth = dormitoryOwner.DateOfBirth,
                ProfilePictureUrl = dormitoryOwner.ProfilePictureUrl,
                UserType = UserType.DormitoryOwner
            };
        }

        private UserDTO MapToUserDTO(Administrator administrator)
        {
            return new UserDTO
            {
                UserId = administrator.AdministratorId,
                Name = administrator.Name,
                Email = administrator.Email,
                PhoneNumber = administrator.PhoneNumber,
                ProfilePictureUrl = administrator.ProfilePictureUrl,
                UserType = UserType.Administrator
            };
        }

        //--------------------------------------------------------------------
        public async Task<Result> ChangePasswordAsync(ChangePasswordDTO changePasswordData)
        {
            switch (changePasswordData.UserType)
            {
                case UserType.Student:
                    var student = await _studentRepository.GetByIdAsync(changePasswordData.UserId);
                    if (student == null || !VerifyPassword(student.Password, changePasswordData.OldPassword))
                        return new Result { IsSuccess = false, Message = "Old password is incorrect" };

                    student.Password = HashPassword(changePasswordData.NewPassword);
                    _studentRepository.Update(student);
                    break;

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.GetByIdAsync(changePasswordData.UserId);
                    if (dormitoryOwner == null || !VerifyPassword(dormitoryOwner.Password, changePasswordData.OldPassword))
                        return new Result { IsSuccess = false, Message = "Old password is incorrect" };

                    dormitoryOwner.Password = HashPassword(changePasswordData.NewPassword);
                    _dormitoryOwnerRepository.Update(dormitoryOwner);
                    break;

                case UserType.Administrator:
                    var administrator = await _administratorRepository.GetByIdAsync(changePasswordData.UserId);
                    if (administrator == null || !VerifyPassword(administrator.Password, changePasswordData.OldPassword))
                        return new Result { IsSuccess = false, Message = "Old password is incorrect" };

                    administrator.Password = HashPassword(changePasswordData.NewPassword);
                    _administratorRepository.Update(administrator);
                    break;

                default:
                    return new Result { IsSuccess = false, Message = "Invalid user type" };
            }

            await SaveChangesAsync(changePasswordData.UserType);
            return new Result { IsSuccess = true, Message = "Password changed successfully" };
        }

        private bool VerifyPassword(string storedPassword, string enteredPassword)
        {
            // Implement your password verification logic here (e.g., hashing and comparing)
            return storedPassword == enteredPassword; // Replace with actual verification
        }

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            return password; // Replace with actual hashing
        }

        public async Task<bool> VerifyOldPasswordAsync(ChangePasswordDTO changePasswordData)
        {
            switch (changePasswordData.UserType)
            {
                case UserType.Student:
                    var student = await _studentRepository.GetByIdAsync(changePasswordData.UserId);
                    return student != null && student.Password == changePasswordData.OldPassword;

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.GetByIdAsync(changePasswordData.UserId);
                    return dormitoryOwner != null && dormitoryOwner.Password == changePasswordData.OldPassword;

                case UserType.Administrator:
                    var administrator = await _administratorRepository.GetByIdAsync(changePasswordData.UserId);
                    return administrator != null && administrator.Password == changePasswordData.OldPassword;

                default:
                    return false;
            }
        }

        //-------------------------------------------------------------------------
        public async Task<UserDTO> GetUserByEmailAsync(string email, UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    var student = await _studentRepository.FindByConditionAsync(s => s.Email == email);
                    if (student != null)
                    {
                        return new UserDTO
                        {
                            UserId = student.StudentId,
                            Name = student.Name,
                            Email = student.Email,
                            PhoneNumber = student.PhoneNumber,
                            Gender = student.Gender,
                            DateOfBirth = student.DateOfBirth,
                            Disabilities = student.Disabilities,
                            ProfilePictureUrl = student.ProfilePictureUrl,
                            UserType = UserType.Student
                        };
                    }
                    break;

                case UserType.DormitoryOwner:
                    var dormitoryOwner = await _dormitoryOwnerRepository.FindByConditionAsync(d => d.Email == email);
                    if (dormitoryOwner != null)
                    {
                        return new UserDTO
                        {
                            UserId = dormitoryOwner.DormitoryOwnerId,
                            Name = dormitoryOwner.Name,
                            Email = dormitoryOwner.Email,
                            PhoneNumber = dormitoryOwner.PhoneNumber,
                            Gender = dormitoryOwner.Gender,
                            DateOfBirth = dormitoryOwner.DateOfBirth,
                            ProfilePictureUrl = dormitoryOwner.ProfilePictureUrl,
                            UserType = UserType.DormitoryOwner
                        };
                    }
                    break;

                case UserType.Administrator:
                    var administrator = await _administratorRepository.FindByConditionAsync(a => a.Email == email);
                    if (administrator != null)
                    {
                        return new UserDTO
                        {
                            UserId = administrator.AdministratorId,
                            Name = administrator.Name,
                            Email = administrator.Email,
                            PhoneNumber = administrator.PhoneNumber,
                            ProfilePictureUrl = administrator.ProfilePictureUrl,
                            UserType = UserType.Administrator
                        };
                    }
                    break;
            }
            return null;
        }
    }
}

