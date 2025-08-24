using E_BangApplication.Exceptions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.ComponentModel.DataAnnotations;
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
                    case ValidationException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case InternalServerErrorException:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                switch (ex)
                {
                    case ValidationException validationException:
                        var errors = validationException.ValidationResult != ValidationResult.Success
                            ? new[] { validationException.ValidationResult.ErrorMessage }
                            : new[] { validationException.Message };
                        problemDetails = new ValidationProblemDetails
                        {
                            Title = "One or more validation errors occurred.",
                            Status = statusCode,
                            Detail = "See the errors property for details.",
                            Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                        };
                        var validationProblemDetails = (ValidationProblemDetails)problemDetails;
                        foreach (var error in errors)
                        {
                            validationProblemDetails.Errors.Add("ValidationErrors", new[] { error });
                        }
                        break;

                    default:
                        problemDetails = new ProblemDetails
                        {
                            Title = ex.GetType().Name,
                            Type = statusCode == 500 ? "Internal Server Error" : ex.GetType().Name,
                            Detail = ex.Message,
                            Status = statusCode,
                            Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                        };
                        break;
                }
                _logger.LogError(ex, JsonSerializer.Serialize(problemDetails));

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
