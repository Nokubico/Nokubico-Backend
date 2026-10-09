using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Nokubico.API.Errors;
using Nokubico.Domain.Validation;

namespace Nokubico.API.Middleware;

/// <summary>
/// Middleware para tratamento centralizado de exceções.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse(
            Status: StatusCodes.Status500InternalServerError,
            Message: "Ocorreu um erro ao processar o pedido.");

        switch (exception)
        {
            case ApiException apiEx:
                response = new ErrorResponse(
                    Status: apiEx.HttpStatusCode,
                    Message: apiEx.Message);
                context.Response.StatusCode = apiEx.HttpStatusCode;
                break;

            case DomainException domainEx:
                response = new ErrorResponse(
                    Status: StatusCodes.Status400BadRequest,
                    Message: domainEx.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;

            case DbUpdateConcurrencyException:
                response = new ErrorResponse(
                    Status: StatusCodes.Status409Conflict,
                    Message: "Ocorreu um conflito ao atualizar os dados. O registo foi modificado por outra operação.");
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                break;

            case DbUpdateException dbEx:
                response = new ErrorResponse(
                    Status: StatusCodes.Status409Conflict,
                    Message: "Ocorreu um erro ao atualizar a base de dados.");
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                break;

            default:
                response = new ErrorResponse(
                    Status: StatusCodes.Status500InternalServerError,
                    Message: "Ocorreu um erro interno do servidor. Por favor, tente novamente mais tarde.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return context.Response.WriteAsync(json);
    }
}

/// <summary>
/// Extensões para registar o middleware de exceções.
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
