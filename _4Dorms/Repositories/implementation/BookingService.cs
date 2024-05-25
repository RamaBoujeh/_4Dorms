using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.implementation
{
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IGenericRepository<Room> _roomRepository;

        public BookingService(IGenericRepository<Booking> bookingRepository, IGenericRepository<Room> roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<bool> BookingAsync(BookingDTO bookingDTO)
        {
            try
            {
                var room = await _roomRepository.GetByIdAsync(bookingDTO.RoomId);
                if (room == null)
                {
                    Console.Error.WriteLine($"Room with ID {bookingDTO.RoomId} not found.");
                    return false;
                }

                if (bookingDTO.RoomType == RoomType.Private && room.NumOfPrivateRooms > 0)
                {
                    room.NumOfPrivateRooms--;
                }
                else if (bookingDTO.RoomType == RoomType.Shared && room.NumOfSharedRooms > 0)
                {
                    room.NumOfSharedRooms--;
                }
                else
                {
                    Console.Error.WriteLine($"No available rooms of type {bookingDTO.RoomType}.");
                    return false; // No available rooms of the selected type
                }

                var booking = new Booking
                {
                    RoomId = bookingDTO.RoomId,
                    DormitoryId = bookingDTO.DormitoryId,
                    StudentId = bookingDTO.StudentId,
                    Duration = bookingDTO.Duration,
                    RoomType = bookingDTO.RoomType
                };

                _bookingRepository.Add(booking);

                var bookingSaveResult = await _bookingRepository.SaveChangesAsync();
                var roomSaveResult = await _roomRepository.SaveChangesAsync();

                Console.WriteLine($"Booking save result: {bookingSaveResult}, Room save result: {roomSaveResult}");

                return bookingSaveResult && roomSaveResult;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in RequestBookingAsync: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                return false;
            }
        }
    }
}
