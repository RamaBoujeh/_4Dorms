namespace _4Dorms.Repositories.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(int cardNumber, DateTime expirationDate, int cvv, decimal amount);
    }
}
