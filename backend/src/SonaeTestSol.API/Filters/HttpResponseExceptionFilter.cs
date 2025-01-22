using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SonaeTestSol.API.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HttpResponseExceptionFilter(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is Exception ex)
            {
                context.Result = _webHostEnvironment.IsDevelopment()
                    ? new ObjectResult(ex.Message)
                    {
                        StatusCode = 500,
                        Value = new
                        {
                            Success = false,
                            Errors = new List<string> { JsonConvert.SerializeObject(ex) }
                        }
                    }
                    : (IActionResult)new ObjectResult(ex.Message)
                    {
                        StatusCode = 500,
                        Value = new
                        {
                            Success = false,
                            Errors = new List<string> { "An unexpected error has occurred. Please try again later." },
                        }
                    };

                context.ExceptionHandled = true;
            }
        }
    }
}
