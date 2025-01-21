using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SonaeTestSol.API.ViewModel;
using SonaeTestSol.Domain.Interfaces.Service;
using SonaeTestSol.Domain.Models;
using SonaeTestSol.Services.Base;
using System.Net.Mime;

namespace SonaeTestSol.API.Controllers.Base
{
    [AllowAnonymous]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json), Produces("application/json")]
    public abstract class ControllerBaseSonae : ControllerBase
    {
        protected readonly IMapper Mapper;
        protected readonly IErrorService ErrorService;

        protected ControllerBaseSonae(IMapper mapper, IErrorService errorService)
        {
            errorService.Clear();
            Mapper = mapper;
            ErrorService = errorService;
        }

        protected bool ValidOperation()
        {
            return !ErrorService.Exists();
        }

        protected IActionResult ModelStateInvalid(ModelStateDictionary modelState)
        {
            IEnumerable<ModelError> errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (ModelError error in errors)
            {
                string message = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                ErrorService.Add(new Error(message));
            }

            return CustomBadRequest();
        }

        protected IActionResult CustomBadRequest()
        {
            return BadRequest(new BadRequestViewModel
            {
                Success = false,
                Errors = ErrorService.Get().Select(n => n.Message)
            });
        }
    }
}
