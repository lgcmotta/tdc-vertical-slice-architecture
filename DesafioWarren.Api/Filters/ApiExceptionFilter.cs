using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DesafioWarren.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace DesafioWarren.Api.Filters
{
    [ExcludeFromCodeCoverage]
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);

            return Task.CompletedTask;
        }

        public override void OnException(ExceptionContext context)
        {
            var response = new Response();

            var exception = context.Exception;

            var requestPath = context.HttpContext.Request.Path;

            Log.Logger.Error(exception, "An exception occurred while processing the HTTP request at: {Path}", requestPath);

            response.AddValidationFailure(new Failure(requestPath, $"Oops! Your request throw an unexpected error. :("));

            context.Result = new BadRequestObjectResult(response);
        }
    }
}