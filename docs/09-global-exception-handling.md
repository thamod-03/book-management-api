# 09 – Add global exception handling

## Goal

Handle unexpected exceptions centrally so controllers do not need repeated try/catch blocks.

## 1. Create the exception handler

Create `Exceptions/GlobalExceptionHandler.cs` in the API project:

```csharp
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
```

This example maps `ArgumentException` to a 400 response and other exceptions to a generic 500 response. In a larger application, prefer mapping known application/domain exceptions explicitly rather than treating every `ArgumentException` as a client error.

Do not expose stack traces, database details, passwords, or internal exception messages in 500 responses.

## 2. Register the handler

In `Program.cs`, before `builder.Build()`:

```csharp
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
```

`AddExceptionHandler<GlobalExceptionHandler>()` registers the custom `IExceptionHandler`. `AddProblemDetails()` registers ASP.NET Core's Problem Details service and provides a fallback response configuration.

## 3. Activate exception handling middleware

After:

```csharp
var app = builder.Build();
```

add:

```csharp
app.UseExceptionHandler();
```

Place it early in the middleware pipeline, before the other middleware that may throw exceptions.

## 4. Understand the difference

- **Registration** tells dependency injection which exception handler to use.
- **Middleware activation** adds exception handling to the HTTP request pipeline.
- Registering the class as an ordinary scoped service alone is not the same as registering it with `AddExceptionHandler<T>()`.

Both registration and middleware activation are required for this setup.

## 5. Cancellation note

A request cancellation is not necessarily an application failure. If the client has disconnected and `RequestAborted` is cancelled, the server may no longer be able to send a response. Avoid trying to write a Problem Details response to a disconnected client. Do not convert cancellation into an ordinary 500 response.

## Checkpoint

Run the API. Confirm that it starts without an exception-handler configuration error. Test normal CRUD operations and, where practical, a controlled invalid operation. Check the server logs for unexpected exceptions.
