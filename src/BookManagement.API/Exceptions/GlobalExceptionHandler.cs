using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
  private readonly ILogger<GlobalExceptionHandler> _logger;

  public GlobalExceptionHandler(
      ILogger<GlobalExceptionHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    _logger.LogError(
        exception,
        "An unhandled exception occurred.");

    var statusCode = exception switch
    {
      ArgumentException => StatusCodes.Status400BadRequest,
      _ => StatusCodes.Status500InternalServerError
    };

    var problemDetails = new ProblemDetails
    {
      Status = statusCode,
      Title = statusCode == 400
            ? "Invalid request"
            : "An unexpected error occurred.",
      Detail = statusCode == 400
            ? exception.Message
            : "Something went wrong while processing your request.",
      Instance = httpContext.Request.Path
    };

    httpContext.Response.StatusCode = statusCode;

    await httpContext.Response.WriteAsJsonAsync(
        problemDetails,
        cancellationToken);

    return true;
  }
}
