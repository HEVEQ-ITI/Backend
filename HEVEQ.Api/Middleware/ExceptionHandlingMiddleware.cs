using System.Net;
using System.Text.Json;
using HEVEQ.Application.Common.Exceptions;

namespace HEVEQ.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, payload) = ex switch
            {
                NotFoundException =>
                    (HttpStatusCode.NotFound, (object)new { message = ex.Message }),

                ForbiddenAccessException =>
                    (HttpStatusCode.Forbidden, (object)new { message = ex.Message }),

                FluentValidation.ValidationException ve =>
                    (HttpStatusCode.BadRequest, (object)new
                    {
                        errors = ve.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                    }),

                _ =>
                    (HttpStatusCode.InternalServerError, (object)new { message = "An unexpected error occurred." })
            };

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}