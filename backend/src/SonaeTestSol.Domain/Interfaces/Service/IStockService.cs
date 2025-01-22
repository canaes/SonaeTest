
namespace SonaeTestSol.Domain.Interfaces.Service
{
    public interface IStockService
    {
        Task<int> Get();
        Task Set(int value);
    }
}