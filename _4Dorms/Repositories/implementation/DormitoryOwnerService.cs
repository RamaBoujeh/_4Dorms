using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.implementation;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using MySqlX.XDevAPI.Common;
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
        private readonly IFileService _fileService;

        public DormitoryOwnerService(IGenericRepository<DormitoryOwner> genericRepositoryDormitoryOwner, IGenericRepository<Dormitory> genericRepositoryDorm,
            IGenericRepository<Room> genericRepositoryRoom, IGenericRepository<DormitoryImage> genericRepositoryDormitoryImage, ILogger<DormitoryService> logger, IFileService fileService)
        {
            _genericRepositoryDormitoryOwner = genericRepositoryDormitoryOwner;
            _genericRepositoryDorm = genericRepositoryDorm;
            _genericRepositoryRoom = genericRepositoryRoom;
            _genericRepositoryDormitoryImage = genericRepositoryDormitoryImage;
            _logger = logger;
            _fileService = fileService;
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

        public async Task<Result> UploadImagesAsync(int dormitoryId, List<IFormFile> images)
        {
            var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                return new Result { IsSuccess = false, Message = "Dormitory not found" };
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "dormitoryImages");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var imageUrl = $"/uploads/dormitoryImages/{fileName}";
                    var dormitoryImage = new DormitoryImage { Url = imageUrl, DormitoryId = dormitoryId };
                    await _genericRepositoryDormitoryImage.Add(dormitoryImage);
                }
            }

            await _genericRepositoryDormitoryImage.SaveChangesAsync();
            return new Result { IsSuccess = true, Message = "Images uploaded successfully" };
        }

        public async Task<Result> DeleteImageAsync(int dormitoryId, string fileName)
        {
            var dormitoryImage = await _genericRepositoryDormitoryImage
                .FindByConditionAsync(img => img.DormitoryId == dormitoryId && img.Url.Contains(fileName));

            if (dormitoryImage == null)
            {
                return new Result { IsSuccess = false, Message = "Image not found or does not belong to the dormitory" };
            }

            var isFileDeleted = _fileService.DeleteFile(fileName);

            if (!isFileDeleted)
            {
                return new Result { IsSuccess = false, Message = "Failed to delete the image file" };
            }

            _genericRepositoryDormitoryImage.Remove(dormitoryImage.ImageId);
            await _genericRepositoryDormitoryImage.SaveChangesAsync();

            return new Result { IsSuccess = true };
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

        public async Task RemoveDormitoryImageAsync(int imageId)
        {
            var image = await _genericRepositoryDormitoryImage.GetByIdAsync(imageId);
            if (image != null)
            {
                _genericRepositoryDormitoryImage.Remove(image.ImageId);
                await _genericRepositoryDormitoryImage.SaveChangesAsync();
            }
        }

        // Update the UpdateDormitoryAsync method in DormitoryOwnerService.cs
        public async Task UpdateDormitoryAsync(int dormitoryId, DormitoryDTO updatedDormitoryDTO, List<IFormFile> newImages)
        {
            _logger.LogInformation("Updating dormitory with ID: {DormitoryId}", dormitoryId);

            var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                _logger.LogWarning("Dormitory with ID {DormitoryId} not found", dormitoryId);
                throw new Exception("Dormitory not found");
            }

            dormitory.DormitoryName = updatedDormitoryDTO.DormitoryName ?? dormitory.DormitoryName;
            dormitory.Location = updatedDormitoryDTO.Location ?? dormitory.Location;
            dormitory.GenderType = updatedDormitoryDTO.GenderType ?? dormitory.GenderType;
            dormitory.City = updatedDormitoryDTO.City ?? dormitory.City;
            dormitory.NearbyUniversity = updatedDormitoryDTO.NearbyUniversity ?? dormitory.NearbyUniversity;
            dormitory.phone = updatedDormitoryDTO.Phone ?? dormitory.phone;
            dormitory.Email = updatedDormitoryDTO.Email ?? dormitory.Email;
            dormitory.DormitoryDescription = updatedDormitoryDTO.DormitoryDescription ?? dormitory.DormitoryDescription;
            dormitory.PriceHalfYear = updatedDormitoryDTO.PriceHalfYear ?? dormitory.PriceHalfYear;
            dormitory.PriceFullYear = updatedDormitoryDTO.PriceFullYear ?? dormitory.PriceFullYear;

            // Ensure ImageUrls is not null
            var updatedImageUrls = updatedDormitoryDTO.ImageUrls ?? new List<string>();

            var imagesToRemove = dormitory.ImageUrls.Where(img => !updatedImageUrls.Contains(img.Url)).ToList();
            foreach (var image in imagesToRemove)
            {
                _genericRepositoryDormitoryImage.Remove(image.ImageId);
            }

            dormitory.ImageUrls = updatedImageUrls.Select(url => new DormitoryImage { Url = url }).ToList();

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "dormitoryImages");
            if (newImages != null && newImages.Any())
            {
                foreach (var image in newImages)
                {
                    if (image.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        var imageUrl = $"/uploads/dormitoryImages/{fileName}";
                        var dormitoryImage = new DormitoryImage { Url = imageUrl, DormitoryId = dormitory.DormitoryId };
                        await _genericRepositoryDormitoryImage.Add(dormitoryImage);
                    }
                }
                await _genericRepositoryDormitoryImage.SaveChangesAsync();
            }

            _genericRepositoryDorm.Update(dormitory);
            await _genericRepositoryDorm.SaveChangesAsync();

            var room = await _genericRepositoryRoom.FindByConditionAsync(r => r.DormitoryId == dormitoryId);
            if (room != null)
            {
                room.PrivateRoom = updatedDormitoryDTO.RoomDTO?.PrivateRoom ?? room.PrivateRoom;
                room.SharedRoom = updatedDormitoryDTO.RoomDTO?.SharedRoom ?? room.SharedRoom;
                room.NumOfPrivateRooms = updatedDormitoryDTO.RoomDTO?.NumOfPrivateRooms ?? room.NumOfPrivateRooms;
                room.NumOfSharedRooms = updatedDormitoryDTO.RoomDTO?.NumOfSharedRooms ?? room.NumOfSharedRooms;

                _genericRepositoryRoom.Update(room);
                await _genericRepositoryRoom.SaveChangesAsync();
            }
        }



    }
}