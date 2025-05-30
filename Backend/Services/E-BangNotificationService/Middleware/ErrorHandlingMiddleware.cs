﻿
namespace E_BangNotificationService.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}
			catch (Exception ex)
			{
				_logger.LogError("{Date}: {ex}", DateTime.Now, ex.Message);
			}
        }
    }
}
