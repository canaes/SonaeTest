
namespace SonaeTestSol.Domain.Interfaces.Service
{
    public interface IStockService
    {
        Task<int> Get();
        Task<int> PayOrder(int quantity);
        Task<int> RefundOrder(int quantity);
    }
}