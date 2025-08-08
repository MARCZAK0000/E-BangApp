using E_BangApplication.Exceptions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Net;
using System.Text.Json;
namespace E_BangAPI.Middleware
{
    public class ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex) // Catch all exceptions
            {
                ProblemDetails problemDetails;
                int statusCode;

                switch (ex)
                {
                    case InvalidDataException:
                    case BadHttpRequestException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UnAuthorizedExceptions:
                    case InvalidCredentialsException:
                    case InvalidTokensException:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case ForbiddenExceptions:
                        statusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    case ArgumentNullException:
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case InternalServerErrorException:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                problemDetails = new ProblemDetails
                {
                    Title = ex.GetType().Name,
                    Type = statusCode == 500 ? "Internal Server Error" : ex.GetType().Name,
                    Detail = ex.InnerException?.Message,
                    Status = statusCode,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };

                _logger.LogError(ex, JsonSerializer.Serialize(problemDetails));

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
