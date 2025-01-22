using Moq;
using Moq.AutoMock;
using SonaeTestSol.Domain.Entities;
using SonaeTestSol.Domain.Interfaces.Repository;
using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Services;
using SonaeTestSol.Services.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace SonaeTestSol.Test
{
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private Mock<IStockService> _mockStockService;
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IErrorService> _mockErrorService;

        public OrderServiceTests()
        {
            _mockStockService = new Mock<IStockService>();
            _mockErrorService = new Mock<IErrorService>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _orderService = new OrderService(_mockErrorService.Object, _mockStockService.Object, _mockOrderRepository.Object);

            // Configurando mocks para retornos esperados
            _mockStockService.Setup(s => s.Get()).ReturnsAsync(100);
            var orders = new List<Order>
            {
                new Order { Quantity = 10, ExpiresOn = DateTime.Now.AddHours(1) },
                new Order { Quantity = 5, ExpiresOn = DateTime.Now.AddHours(1) }
            };

            // Setting repository to use initial data
            _mockOrderRepository.Setup(repo => repo.GetAll()).ReturnsAsync(orders);

            _mockOrderRepository.Setup(repo => repo.Add(It.IsAny<Order>())).Callback<Order>(o => orders.Add(o));

            _mockOrderRepository.Setup(repo => repo.Update(It.IsAny<Order>())).Callback<Order>(o =>
            {
                var index = orders.FindIndex(ord => ord.Id == o.Id);
                if (index != -1) orders[index] = o;
            });
        }

        [Fact]
        [Trait("Order", "Order Tests")]
        public async Task AddOrder_ShouldReturnFalse_WhenValidationFails()
        {
            // Arrange
            int quantity = 500;

            // Act
            var result = await _orderService.AddOrder(quantity);

            // Assert
            Assert.False(result.Item1);
            Assert.Equal(85, result.Item2); // 100 produtos no estoque - 15 já reservados = 85
        }

        [Fact]
        public async Task AddOrder_ShouldReturnTrue_WhenValidationSucceeds()
        {
            // Arrange
            int quantity = 10;

            // Act
            var result = await _orderService.AddOrder(quantity);

            // Assert
            Assert.True(result.Item1);
            Assert.Equal(75, result.Item2); // 100 produtos no estoque - 25 já reservados = 75
        }

        [Fact]
        [Trait("Order", "Order Tests")]
        public async Task AddOrder_ShouldSetExpiration_WhenOrderIsActive()
        {
            // Arrange
            int quantity = 10;

            // Act
            var Id = Guid.NewGuid();
            var result = await _orderService.AddOrder(quantity, Id);
            var order = _orderService.GetAll(0, 10).Result.FirstOrDefault(x => x.Id == Id);

            // Assert
            Assert.Equal(DateTime.Now.AddHours(1).Hour, order.ExpiresOn.Hour);
        }

        [Fact]
        [Trait("Order", "Order Tests")]
        public async Task CompleteOrder_ShouldReturnFalse_WhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            // Act
            var result = await _orderService.CompleteOrder(orderId);

            // Assert
            Assert.False(result.Item1);
        }

        [Fact]
        [Trait("Order", "Complete Order Tests")]
        public async Task CompleteOrder_ShouldReturnTrue_WhenOrderIsCompleted()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                Quantity = 6,
                Status = Domain.Enumerators.Enumerators.StatusOrder.Active
            };

            await _orderService.AddOrder(order.Quantity, order.Id);

            _mockOrderRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Order> { order });

            // Act
            var result = await _orderService.CompleteOrder(order.Id);

            // Verificando a atualização
            var updatedOrders = await _mockOrderRepository.Object.GetAll();
            var updatedOrder = updatedOrders.FirstOrDefault(o => o.Id == order.Id);

            // Assert
            Assert.True(result.Item1);
            Assert.NotNull(updatedOrder);
            Assert.Equal(Domain.Enumerators.Enumerators.StatusOrder.Completed, updatedOrder.Status);
            Assert.False(_mockErrorService.Object.Exists());
        }

        [Fact]
        [Trait("Order", "Complete Order Tests")]
        public async Task CompleteOrder_ShouldReturnFalse_WhenOrderIsCompleted()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Quantity = 6,
                Status = Domain.Enumerators.Enumerators.StatusOrder.Active
            };

            await _orderService.AddOrder(order.Quantity, order.Id);

            _mockOrderRepository.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Order> { order });

            // Act
            var result = await _orderService.CompleteOrder(orderId);

            // Assert
            Assert.False(result.Item1);            
        }

        [Fact]
        [Trait("Order", "Process Order Tests")]
        public async Task ProcessExpireOrder_ShouldUpdateExpiredOrders()
        {
            // Arrange
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Quantity = 6,
            };
            await _orderService.AddOrder(order.Quantity, order.Id, DateTime.Now.AddHours(-2));

            // Act
            await _orderService.ProcessExpireOrder();

            // Verificando se o pedido expirado foi atualizado corretamente
            var updatedOrders = await _mockOrderRepository.Object.GetAll();
            var listOrder = updatedOrders.Where(o => o.ExpiresOn < DateTime.Now && o.Status != Domain.Enumerators.Enumerators.StatusOrder.Expired).ToList();
            var expiredOrder = _orderService.GetAll(0, 10).Result.FirstOrDefault(x => x.Id == order.Id);

            // Assert
            Assert.Empty(listOrder);
            Assert.Equal(Domain.Enumerators.Enumerators.StatusOrder.Expired, expiredOrder.Status);
        }

    }
}