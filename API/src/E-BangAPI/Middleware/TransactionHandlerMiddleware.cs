using E_BangApplication.Attributes;
using E_BangInfrastructure.Database;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_BangAPI.Middleware
{
    public class TransactionHandlerMiddleware(ProjectDbContext dbContext) : IMiddleware
    {
        private readonly ProjectDbContext _dbContext = dbContext;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //await next.Invoke(context);
            if (context.Request.Method == "GET")
            {
                await next.Invoke(context);
                return;
            }

            var enpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attriubte = enpoint?.Metadata.GetMetadata<TransactionAttribute>();

            if (attriubte == null)
            {
                await next.Invoke(context);
            }
            IDbContextTransaction dbTransaction = null!;
            try
            {
                using (dbTransaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    await next.Invoke(context);
                    await dbTransaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
            finally
            {
                dbTransaction.Dispose();
            }
        }
    }
}
