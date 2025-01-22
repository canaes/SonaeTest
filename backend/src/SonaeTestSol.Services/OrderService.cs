using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Interfaces;
using SonaeTestSol.Domain.Interfaces.Repository;
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
        
        private readonly IStockService _stockService;
        private readonly IOrderRepository _orderRepository;
        const int reserveInterval = 1;

        public OrderService(IErrorService errorService, IStockService stockService, IOrderRepository orderRepository) : base(errorService)
        {
            _stockService = stockService;
            _orderRepository = orderRepository;
        }

        public async Task<ICollection<Order>> GetAll(int skip, int quantity)
        {
            
            return await Task.FromResult(_orderRepository.GetAll().Result.Skip(skip).Take(quantity).ToList());
            
        }

        public async Task<int> GetQuantityAvailable()
        {
            var stockQt = await _stockService.Get();
            var orderActComp = _orderRepository.GetAll().Result.Where(x => x.Status != Domain.Enumerators.Enumerators.StatusOrder.Expired).Sum(x => x.Quantity);

            return stockQt - orderActComp;
        }

        public async Task<(bool, int)> AddOrder(int quantity, Guid? id = null, DateTime? expires = null)
        {
            var entity = new Order
            {
                Quantity = quantity,
                Status = Domain.Enumerators.Enumerators.StatusOrder.Active,
                ExpiresOn = DateTime.Now.AddHours(reserveInterval),
            };
            if (id.HasValue)
            {
                entity.Id = id.Value;
            }
            if (expires.HasValue)
            {
                entity.ExpiresOn = expires.Value;
            }

            if (!RunValidationService(entity))
            {
                return (false, GetQuantityAvailable().Result);
            }

            await _orderRepository.Add(entity);
            return (true, GetQuantityAvailable().Result);
        }

        public bool RunValidationService(Order entity)
        {
            if (!RunValidation(new OrderValidation(GetQuantityAvailable().Result), entity))
            {
                return false;
            }
            return true;
        }


        public async Task<(bool, int)> CompleteOrder(Guid Id)
        {
            var order = _orderRepository.GetAll().Result
                            .Where(x => x.Id.Equals(Id) 
                                && x.Status == Domain.Enumerators.Enumerators.StatusOrder.Active)
                            .FirstOrDefault();

            if (order is null)
            {
                ErrorService.Add(new Error("Order not found or cannot be completed"));
                return (false, GetQuantityAvailable().Result);
            }

            order.Status = Domain.Enumerators.Enumerators.StatusOrder.Completed;
            await _orderRepository.Update(order);
            
            return await Task.FromResult((true, GetQuantityAvailable().Result));
        }


        public async Task ProcessExpireOrder()
        {
            var order = _orderRepository.GetAll().Result.Where(x => x.Status == Domain.Enumerators.Enumerators.StatusOrder.Active && x.ExpiresOn < DateTime.Now).ToList();

            foreach (var entity in order)
            {
                entity.Status = Domain.Enumerators.Enumerators.StatusOrder.Expired;
            }
            await Task.CompletedTask;
        }
    }
}
