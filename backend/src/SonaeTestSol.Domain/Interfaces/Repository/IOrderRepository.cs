using SonaeTestSol.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonaeTestSol.Domain.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task<int> Add(Order o);
        Task<List<Order>> GetAll();
        Task<int> GetQuantityOrders();
        Task Update(Order o);
    }
}
