using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.implementation
{
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;

        public BookingService(IGenericRepository<Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> RequestBookingAsync(BookingDTO bookingDTO)
        {
            var booking = new Booking
            {
                RoomId = bookingDTO.RoomId,
                DormitoryId = bookingDTO.DormitoryId,
                CheckInDate = bookingDTO.CheckInDate,
                CheckOutDate = bookingDTO.CheckOutDate,
                Status = Status.Pending
            };

            _bookingRepository.Add(booking);
            return await _bookingRepository.SaveChangesAsync();
        }

        public async Task<bool> ConfirmBookingAsync(int bookingId, bool isApproved, int dormitoryOwnerId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                return false;
            }

            if (isApproved)
            {
                booking.Status = Status.Approved;
            }
            else
            {
                booking.Status = Status.Rejected;
            }

            booking.DormitoryOwnerId = dormitoryOwnerId;
            _bookingRepository.Update(booking);

            return await _bookingRepository.SaveChangesAsync();
        }
    }
}
   