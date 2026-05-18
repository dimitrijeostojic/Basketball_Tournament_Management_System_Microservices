using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace StadiumService.Middleware;

public sealed class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .Select(e => new { property = e.PropertyName, message = e.ErrorMessage })
                .ToList();

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status = 400,
                title = "Validation failed",
                errors
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            ProblemDetails problemDetails = new()
            {
                Status = context.Response.StatusCode,
                Type = "Server error",
                Title = ex.GetType().Name,
                Detail = ex.Message + " | " + ex.InnerException?.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
