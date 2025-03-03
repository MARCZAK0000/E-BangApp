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
            #region 400 ----------- Bad Request
			catch(InvalidDataException err)
			{
				var problemDetails = new ProblemDetails
				{
					Title = nameof(InvalidDataException),
					Type = "Invalid input data",
					Detail = err.Message,
					Status = (Int32)HttpStatusCode.BadRequest,
					Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
				};
				var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
				await context.Response.WriteAsync(json);	
			}
			catch(BadHttpRequestException err)
			{
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(BadHttpRequestException),
                    Type = "Invalid Request ",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.BadRequest,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }
            #endregion 400 ----------- Bad Request

            #region 401 ----------- UnAuthorized

            catch (UnAuthorizedExceptions err)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(UnAuthorizedExceptions),
                    Type = "UnAuthorized",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.Unauthorized,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }

            catch(InvalidCredentialsException err)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(InvalidCredentialsException),
                    Type = "Invalid UserName or Password",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.Unauthorized,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }

            catch (InvalidTokensException err)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(InvalidTokensException),
                    Type = "Invalid Token",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.Unauthorized,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }
            #endregion 401 ----------- UnAuthorized

            #region 403 ----------- Fobid

            catch (ForbiddenExceptions err)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(ForbiddenExceptions),
                    Type = "Forbidden",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.Forbidden,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }

            #endregion 403 ----------- Fobid

            #region 404 ----------- NotFound
            catch (ArgumentNullException err)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(ArgumentNullException),
                    Type = "Data Not Found",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.NotFound,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }
            #endregion 404 ----------- NotFound

            #region 500 ----------- Internal Server Error
            catch (InternalServerErrorException err)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(InternalServerErrorException),
                    Type = "Internal Server Error",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.InternalServerError,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }
			catch (Exception err)
			{
                var problemDetails = new ProblemDetails
                {
                    Title = nameof(InternalServerErrorException),
                    Type = "Internal Server Error",
                    Detail = err.Message,
                    Status = (Int32)HttpStatusCode.InternalServerError,
                    Instance = context.Features.Get<IEndpointFeature>()?.Endpoint?.ToString()
                };
                var json = JsonSerializer.Serialize(problemDetails);
                _logger.LogError(json);
                await context.Response.WriteAsync(json);
            }
            #endregion 500 ----------- Internal Server Error
        }
    }
}
