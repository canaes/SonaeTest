using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Domain.Models;
using SonaeTestSol.Services.Base;
using SonaeTestSol.Services.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonaeTestSol.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private List<Order> Orders = new List<Order>();
        private readonly IStockService _stockService;
        const int intervalDelay = 1;

        public OrderService(IErrorService errorService, IStockService stockService) : base(errorService)
        {
            _stockService = stockService;
        }

        public async Task<ICollection<Order>> GetAll(int skip, int quantity)
        {
            return Orders.Skip(skip).Take(quantity).ToList();
        }

        public async Task<(bool, int)> AddOrder(int quantity)
        {
            var entity = new Order
            {
                Quantity = quantity,
                Status = Domain.Enumerators.Enumerators.StatusOrder.Active,
                ExpiresOn = DateTime.Now.AddHours(intervalDelay),
            };

            if (!RunValidation(new OrderValidation(_stockService.Get().Result), entity))
            {
                return (false, _stockService.Get().Result);
            }


            Orders.Add(entity);
            await _stockService.PayOrder(quantity);
            return (true, _stockService.Get().Result);
        }

        public async Task<(bool, int)> CompleteOrder(Guid Id)
        {
            var order = Orders
                            .Where(x => x.Id.Equals(Id) 
                                && x.Status == Domain.Enumerators.Enumerators.StatusOrder.Active)
                            .FirstOrDefault();

            if (order is null)
            {
                ErrorService.Add(new Error("Order not found or cannot be completed"));
                return (false, _stockService.Get().Result);
            }

            order.Status = Domain.Enumerators.Enumerators.StatusOrder.Completed;
            return (true, _stockService.Get().Result);
        }
    }
}
