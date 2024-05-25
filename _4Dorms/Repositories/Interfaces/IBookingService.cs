using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IBookingService
    {
        Task<bool> BookingAsync(BookingDTO bookingDTO);

    }
}
