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

        public async Task<bool> DeleteUserProfileByAdminAsync(int userId, UserType userType)
        {
            switch (userType)
            {
                case UserType.Student:
                    var student = await _genericRepositoryStudent.GetByIdAsync(userId);
                    if (student == null)
                        return false;
                    _genericRepositoryStudent.Remove(userId);
                    return await _genericRepositoryStudent.SaveChangesAsync();

                case UserType.DormitoryOwner:
                    var dormOwner = await _genericRepositoryDormitoryOwner.GetByIdAsync(userId);
                    if (dormOwner == null)
                        return false;
                    _genericRepositoryDormitoryOwner.Remove(userId);
                    return await _genericRepositoryDormitoryOwner.SaveChangesAsync();
                default:
                    throw new ArgumentException("Invalid user type.");
            }
        }
    }
}

