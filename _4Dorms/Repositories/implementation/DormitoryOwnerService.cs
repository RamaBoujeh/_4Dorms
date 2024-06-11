using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.implementation;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using System.Linq;
using System.Threading.Tasks;

namespace _4Dorms.Repositories.Implementation
{
    public class DormitoryOwnerService : IDormitoryOwnerService
    {
        private readonly IGenericRepository<DormitoryOwner> _genericRepositoryDormitoryOwner;
        private readonly IGenericRepository<Dormitory> _genericRepositoryDorm;
        private readonly IGenericRepository<Room> _genericRepositoryRoom;
        private readonly IGenericRepository<DormitoryImage> _genericRepositoryDormitoryImage;
        private readonly ILogger<DormitoryService> _logger;

        public DormitoryOwnerService(IGenericRepository<DormitoryOwner> genericRepositoryDormitoryOwner, IGenericRepository<Dormitory> genericRepositoryDorm,
            IGenericRepository<Room> genericRepositoryRoom, IGenericRepository<DormitoryImage> genericRepositoryDormitoryImage, ILogger<DormitoryService> logger)
        {
            _genericRepositoryDormitoryOwner = genericRepositoryDormitoryOwner;
            _genericRepositoryDorm = genericRepositoryDorm;
            _genericRepositoryRoom = genericRepositoryRoom;
            _genericRepositoryDormitoryImage = genericRepositoryDormitoryImage;
            _logger = logger;
        }

        public async Task<IEnumerable<DormitoryOwner>> GetAllOwnersAsync()
        {
            return await _genericRepositoryDormitoryOwner.GetAllAsync();
        }

        public async Task<DormitoryOwnerDTO> GetOwnerByIdAsync(int ownerId)
        {
            var owner = await _genericRepositoryDormitoryOwner.GetByIdAsync(ownerId);
            if (owner == null) return null;

            return new DormitoryOwnerDTO
            {
                Name = owner.Name,
                Email = owner.Email,
                Password = owner.Password,
                PhoneNumber = owner.PhoneNumber,
                Gender = owner.Gender,
                DateOfBirth = owner.DateOfBirth
            };
        }

        public async Task SubmitDormitoryForApprovalAsync(DormitorySubmitDTO dormitoryDTO, int dormitoryOwnerId, List<IFormFile> images)
        {
            var dormitory = new Dormitory
            {
                DormitoryOwnerId = dormitoryOwnerId,
                DormitoryName = dormitoryDTO.DormitoryName,
                GenderType = dormitoryDTO.GenderType,
                City = dormitoryDTO.City,
                NearbyUniversity = dormitoryDTO.NearbyUniversity,
                phone = dormitoryDTO.Phone,
                Email = dormitoryDTO.Email,
                DormitoryDescription = dormitoryDTO.DormitoryDescription,
                PriceFullYear = dormitoryDTO.PriceFullYear,
                PriceHalfYear = dormitoryDTO.PriceHalfYear,
                Location = dormitoryDTO.Location,
                Status = DormitoryStatus.Pending
            };

            await _genericRepositoryDorm.Add(dormitory);
            await _genericRepositoryDorm.SaveChangesAsync();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "dormitoryImages");
            if (!Directory.Exists(uploadsFolder))
            {
                _logger.LogInformation("Creating directory: {UploadsFolder}", uploadsFolder);
                Directory.CreateDirectory(uploadsFolder);
            }

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        _logger.LogInformation("Saving file: {FilePath}", filePath);

                        try
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                                _logger.LogInformation("File saved successfully: {FilePath}", filePath);
                            }

                            var imageUrl = $"/uploads/dormitoryImages/{fileName}";
                            var dormitoryImage = new DormitoryImage { Url = imageUrl, DormitoryId = dormitory.DormitoryId };
                            await _genericRepositoryDormitoryImage.Add(dormitoryImage);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to save file: {FilePath}", filePath);
                            throw; // Rethrow to handle the error accordingly
                        }
                    }
                }
                await _genericRepositoryDormitoryImage.SaveChangesAsync();
            }

            if (dormitoryDTO.RoomDTO != null)
            {
                var room = new Room
                {
                    PrivateRoom = dormitoryDTO.RoomDTO.PrivateRoom ?? false,
                    SharedRoom = dormitoryDTO.RoomDTO.SharedRoom ?? false,
                    NumOfPrivateRooms = dormitoryDTO.RoomDTO.NumOfPrivateRooms ?? 0,
                    NumOfSharedRooms = dormitoryDTO.RoomDTO.NumOfSharedRooms ?? 0,
                    DormitoryId = dormitory.DormitoryId
                };

                await _genericRepositoryRoom.Add(room);
                await _genericRepositoryRoom.SaveChangesAsync();
            }
        }


        public async Task UpdateDormitoryAsync(int dormitoryId, DormitoryDTO updatedDormitoryDTO)
        {
            var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                throw new Exception("Dormitory not found");
            }

            dormitory.DormitoryName = updatedDormitoryDTO.DormitoryName;
            dormitory.Location = updatedDormitoryDTO.Location;
            dormitory.GenderType = updatedDormitoryDTO.GenderType;
            dormitory.City = updatedDormitoryDTO.City;
            dormitory.NearbyUniversity = updatedDormitoryDTO.NearbyUniversity;
            dormitory.phone = updatedDormitoryDTO.Phone;
            dormitory.Email = updatedDormitoryDTO.Email;
            dormitory.DormitoryDescription = updatedDormitoryDTO.DormitoryDescription;
            dormitory.PriceHalfYear = updatedDormitoryDTO.PriceHalfYear;
            dormitory.PriceFullYear = updatedDormitoryDTO.PriceFullYear;

            var imagesToRemove = dormitory.ImageUrls.ToList();
            foreach (var image in imagesToRemove)
            {
                _genericRepositoryDormitoryImage.Remove(image.ImageId);
            }

            dormitory.ImageUrls.Clear();
            if (updatedDormitoryDTO.ImageUrls != null)
            {
                dormitory.ImageUrls = updatedDormitoryDTO.ImageUrls.Select(url => new DormitoryImage { Url = url }).ToList();
            }

            _genericRepositoryDorm.Update(dormitory);
            await _genericRepositoryDorm.SaveChangesAsync();

            // Update room details
            var room = await _genericRepositoryRoom.FindByConditionAsync(r => r.DormitoryId == dormitoryId);
            if (room != null)
            {
                room.PrivateRoom = updatedDormitoryDTO.RoomDTO.PrivateRoom;
                room.SharedRoom = updatedDormitoryDTO.RoomDTO.SharedRoom;
                room.NumOfPrivateRooms = updatedDormitoryDTO.RoomDTO.NumOfPrivateRooms;
                room.NumOfSharedRooms = updatedDormitoryDTO.RoomDTO.NumOfSharedRooms;

                _genericRepositoryRoom.Update(room);
                await _genericRepositoryRoom.SaveChangesAsync();
            }
        }


        public async Task DeleteDormitoryAsync(int dormitoryId)
        {
            var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                throw new Exception("Dormitory not found");
            }

            _genericRepositoryDorm.Remove(dormitoryId);
            await _genericRepositoryDorm.SaveChangesAsync();
        }


    }
}