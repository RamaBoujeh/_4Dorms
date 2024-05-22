using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.implementation
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IGenericRepository<Administrator> _genericRepositoryAdmin;
        private readonly IGenericRepository<Dormitory> _genericRepositoryDorm;
        private readonly IGenericRepository<Student> _genericRepositoryStudent;
        private readonly IGenericRepository<DormitoryOwner> _genericRepositoryDormitoryOwner;
        private readonly IGenericRepository<FavoriteList> _genericRepositoryFavoriteList;

        public AdministratorService(IGenericRepository<Administrator> genericRepositoryAdmin,
            IGenericRepository<Dormitory> genericRepositoryDorm,
            IGenericRepository<Student> genericRepositoryStudent,
            IGenericRepository<DormitoryOwner> genericRepositoryDormitoryOwner)
        {
            _genericRepositoryAdmin = genericRepositoryAdmin;
            _genericRepositoryDorm = genericRepositoryDorm;
            _genericRepositoryStudent = genericRepositoryStudent;
            _genericRepositoryDormitoryOwner = genericRepositoryDormitoryOwner;
        }

        public async Task ReviewDormitoryAsync(int dormitoryId, bool approved, int administratorId)
        {
            var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            if (dormitory != null)
            {
                if (approved)
                {
                    dormitory.Status = DormitoryStatus.Approved;
                    dormitory.AdministratorId = administratorId;
                }
                else
                {
                    dormitory.Status = DormitoryStatus.Rejected;
                }
                await _genericRepositoryDorm.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteUserProfileAsync(int userId, UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    var student = await _genericRepositoryStudent.GetByIdAsync(userId);
                    if (student == null)
                        return false;

                    // Remove the student's favorite list
                    await RemoveFavoriteListForUser(userId, UserType.Student);

                    _genericRepositoryStudent.Remove(userId);
                    return await _genericRepositoryStudent.SaveChangesAsync();

                case UserType.DormitoryOwner:
                    var dormOwner = await _genericRepositoryDormitoryOwner.GetByIdAsync(userId);
                    if (dormOwner == null)
                        return false;

                    // Remove the dormitory owner's favorite list
                    await RemoveFavoriteListForUser(userId, UserType.DormitoryOwner);

                    _genericRepositoryDormitoryOwner.Remove(userId);
                    return await _genericRepositoryDormitoryOwner.SaveChangesAsync();

                default:
                    throw new ArgumentException("Invalid user type.");
            }
        }

        private async Task RemoveFavoriteListForUser(int favoriteListId, UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    var studentFavoriteList = await _genericRepositoryFavoriteList.GetByIdAsync(favoriteListId);
                    if (studentFavoriteList != null)
                    {
                        _genericRepositoryFavoriteList.Remove(favoriteListId);
                        await _genericRepositoryFavoriteList.SaveChangesAsync();
                    }
                    break;

                case UserType.DormitoryOwner:
                    var dormitoryOwnerFavoriteList = await _genericRepositoryFavoriteList.GetByIdAsync(favoriteListId);
                    if (dormitoryOwnerFavoriteList != null)
                    {
                        _genericRepositoryFavoriteList.Remove(favoriteListId);
                        await _genericRepositoryFavoriteList.SaveChangesAsync();
                    }
                    break;

                default:
                    break;
            }
        }

        public async Task<bool> UpdateDormStatusAsync(int dormId, DormitoryStatus status)
        {
            var dorm = await _genericRepositoryDorm.FindByConditionAsync(d => d.DormitoryId == dormId);
            if (dorm == null)
            {
                Console.WriteLine($"Dormitory not found with DormId={dormId}");
                return false;
            }
            dorm.Status = status;
            _genericRepositoryDorm.Update(dorm);
            return await _genericRepositoryDorm.SaveChangesAsync();
        }

    }
}

