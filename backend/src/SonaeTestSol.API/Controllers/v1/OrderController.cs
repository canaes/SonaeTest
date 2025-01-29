using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonaeTestSol.API.Controllers.Base;
using SonaeTestSol.API.ViewModel;
using SonaeTestSol.Domain.DTO;
using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Services.Base;
using System.Net.WebSockets;

namespace SonaeTestSol.API.Controllers.v1
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/order")]
    public class OrderController : ControllerBaseSonae
    {
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService, IErrorService errorService) : base(mapper, errorService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResponseViewModel<List<OrderDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestViewModel), StatusCodes.Status400BadRequest)]
        [HttpGet("get-all")]
        public async Task<IActionResult> Get([FromQuery] int skip, [FromQuery] int quantity)
        {
            if (quantity <= 0) quantity = 10;

            return Ok(new ResponseViewModel<List<OrderDTO>>
            {
                Success = true,
                Data = Mapper.Map<List<OrderDTO>>(await _orderService.GetAll(skip, quantity))
            });
        }

        [ProducesResponseType(typeof(ResponseViewModel<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestViewModel), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] PayloadOrderViewModel paylod)
        {
            (bool result, int qtt) = await _orderService.AddOrder(paylod.quantity);

            if (!ValidOperation()) return CustomBadRequest();

            return Ok(new ResponseViewModel<int>
            {
                Success = result,
                Data = qtt
            });
        }


        [ProducesResponseType(typeof(ResponseViewModel<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestViewModel), StatusCodes.Status400BadRequest)]
        [HttpPatch("complete/{orderId:guid}")]
        public async Task<IActionResult> CompleteOrder([FromRoute] Guid orderId)
        {
            (bool result, int qtt) = await _orderService.CompleteOrder(orderId);

            if (!ValidOperation()) return CustomBadRequest();

            return NoContent();
        }

    }
}
