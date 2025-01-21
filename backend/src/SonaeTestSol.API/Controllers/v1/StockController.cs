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
    [Route("api/v{version:apiVersion}/stock")]
    public class StockController : ControllerBaseSonae
    {
        private readonly IOrderService _orderService;
        private readonly IStockService _stockService;

        public StockController(IMapper mapper, IOrderService orderService, IStockService stockService, IErrorService errorService) : base(mapper, errorService)
        {
            _orderService = orderService;
            _stockService = stockService;
        }

        [ProducesResponseType(typeof(ResponseViewModel<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestViewModel), StatusCodes.Status400BadRequest)]
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var response = await _stockService.Get(_orderService.GetQuantityActiveComplete().Result);

            return Ok(new ResponseViewModel<int>
            {
                Success = true,
                Data = response
            });
        }
    }
}
