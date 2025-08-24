using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyCustomMediator.Deleagate;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.Pipelines
{
    public class ValidationPipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class, new()
    {

        private readonly IServiceProvider _serviceProvider;

        public ValidationPipeline(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> SendToPipeline(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var actualRequestType = request.GetType(); 
            var validatorType = typeof(IValidator<>).MakeGenericType(actualRequestType);
            var validator = _serviceProvider.GetServices(validatorType).Cast<IValidator>();

            if(validator == null || !validator.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            if(context == null)
            {
                return await next();
            }
            var validationResults = await Task.WhenAll(validator.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults
                .Where(r => r != null)
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
            return await next();
        }
    }
}