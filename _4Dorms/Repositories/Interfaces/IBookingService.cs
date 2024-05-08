using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IBookingService
    {
        Task<bool> RequestBookingAsync(BookingDTO bookingDTO);
        Task<bool> ConfirmBookingAsync(int bookingId, bool isApproved, int dormitoryOwnerId);

    }
}
