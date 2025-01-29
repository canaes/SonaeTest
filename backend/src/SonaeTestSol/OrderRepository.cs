using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Interfaces.Repository;
using System.Net.Http.Headers;

namespace SonaeTestSol
{
    public class OrderRepository: IOrderRepository
    {
        public List<Order> Orders { get; private set; }

        public OrderRepository()
        {
            Orders = new List<Order>();
        }

        public async Task<List<Order>> GetAll()
        {
            return Orders;
        }

        public async Task<int> GetQuantityOrders()
        {
            return Orders.Where(x => x.Status != Domain.Enumerators.Enumerators.StatusOrder.Expired).Sum(x => x.Quantity);
        }

        public async Task<int> Add(Order o)
        {
            Orders.Add(o);
            return Orders.Count();
        }

        public async Task Update(Order o)
        {
            var index = Orders.FindIndex(x => x.Id == o.Id);
            if (index != -1) Orders[index] = o;
        }

    }
}
