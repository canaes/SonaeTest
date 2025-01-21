

using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Models;

namespace SonaeTestSol.Domain.Interfaces.Service
{
    public interface IOrderService
    {
        Task<(bool, int)> AddOrder(int quantity);
        Task<(bool, int)> CompleteOrder(Guid Id);
        Task<ICollection<Order>> GetAll(int skip, int quantity);
        Task ProcessExpireOrder();
    }
}