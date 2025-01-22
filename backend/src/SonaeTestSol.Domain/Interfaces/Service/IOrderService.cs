

using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Models;

namespace SonaeTestSol.Domain.Interfaces.Service
{
    public interface IOrderService
    {
        Task<(bool, int)> AddOrder(int quantity, Guid? id = null, DateTime? expires = null);
        Task<(bool, int)> CompleteOrder(Guid Id);
        Task<ICollection<Order>> GetAll(int skip, int quantity);
        Task<int> GetQuantityAvailable();
        Task ProcessExpireOrder();
    }
}