using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> _logger;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError($"An unhandled exception occurred: {context.Exception}");
        context.Result = new ObjectResult("An error occurred. Please try again later.")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}
