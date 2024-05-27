using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace _4Dorms.Repositories.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IGenericRepository<Booking> bookingRepository, IGenericRepository<Room> roomRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public async Task<bool> BookingAsync(BookingDTO bookingDTO)
        {
            await _bookingRepository.BeginTransactionAsync(); // Start transaction

            try
            {
                var room = await _roomRepository.Query()
                    .Include(r => r.Dormitory)
                    .FirstOrDefaultAsync(r => r.RoomID == bookingDTO.RoomId);

                if (room == null)
                {
                    _logger.LogError($"Room with ID {bookingDTO.RoomId} not found.");
                    await _bookingRepository.RollbackTransactionAsync();
                    return false;
                }

                if (room.Dormitory == null)
                {
                    _logger.LogError($"Dormitory for Room ID {bookingDTO.RoomId} not found.");
                    await _bookingRepository.RollbackTransactionAsync();
                    return false;
                }

                bool roomUpdated = false;

                if (bookingDTO.RoomType == RoomType.Private)
                {
                    if (room.NumOfPrivateRooms > 0)
                    {
                        room.NumOfPrivateRooms--;
                        roomUpdated = true;
                    }
                    else
                    {
                        _logger.LogError($"No available private rooms. Room ID: {bookingDTO.RoomId}");
                        await _bookingRepository.RollbackTransactionAsync();
                        return false;
                    }
                }
                else if (bookingDTO.RoomType == RoomType.Shared)
                {
                    if (room.NumOfSharedRooms > 0)
                    {
                        room.NumOfSharedRooms--;
                        roomUpdated = true;
                    }
                    else
                    {
                        _logger.LogError($"No available shared rooms. Room ID: {bookingDTO.RoomId}");
                        await _bookingRepository.RollbackTransactionAsync();
                        return false;
                    }
                }
                else
                {
                    _logger.LogError($"Invalid room type. Room ID: {bookingDTO.RoomId}, Room Type: {bookingDTO.RoomType}");
                    await _bookingRepository.RollbackTransactionAsync();
                    return false;
                }

                if (roomUpdated)
                {
                    _roomRepository.Update(room);
                }

                var booking = new Booking
                {
                    RoomId = bookingDTO.RoomId,
                    DormitoryId = bookingDTO.DormitoryId,
                    StudentId = bookingDTO.StudentId,
                    Duration = bookingDTO.Duration,
                    RoomType = bookingDTO.RoomType,
                    DormitoryOwnerId = room.Dormitory.DormitoryOwnerId,
                };

                await _bookingRepository.Add(booking);

                var bookingSaveResult = false;
                var roomSaveResult = true;

                try
                {
                    bookingSaveResult = await _bookingRepository.SaveChangesAsync();
                    _logger.LogInformation($"Booking save result: {bookingSaveResult}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving booking: {ex.Message}");
                    _logger.LogError(ex.StackTrace);
                }

                try
                {
                    roomSaveResult = await _roomRepository.SaveChangesAsync();
                    _logger.LogInformation($"Room save result: {roomSaveResult}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving room: {ex.Message}");
                    _logger.LogError(ex.StackTrace);
                }

                if (bookingSaveResult)
                {
                    await _bookingRepository.CommitTransactionAsync(); // Commit transaction if all operations succeed
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to save booking or update room.");
                    await _bookingRepository.RollbackTransactionAsync(); // Rollback transaction if any operation fails
                    return false;
                }
            }
            catch (DbUpdateException dbEx)
            {
                await _bookingRepository.RollbackTransactionAsync(); // Ensure rollback on exception
                _logger.LogError($"Database update error in BookingAsync: {dbEx.Message}");
                _logger.LogError(dbEx.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                await _bookingRepository.RollbackTransactionAsync(); // Ensure rollback on exception
                _logger.LogError($"Error in BookingAsync: {ex.Message}");
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> HasCompletedBookingAsync(int dormitoryId, int studentId)
        {
            return await _bookingRepository.Query()
                .AnyAsync(b => b.DormitoryId == dormitoryId && b.StudentId == studentId);
        }

    }
}
