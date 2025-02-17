using Domain.Enumerations;
using FluentValidation;
using MediatR;

namespace SupperDotNetApi.Middleware;

public class ValidationBehaviorMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviorMiddleware(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Check if any validators are registered for the request
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(result => result.Errors).Where(f => f != null).ToList();

            if (failures.Count > 0)
            {
                var response = Activator.CreateInstance(typeof(TResponse), FeedBackCode.ValidationNotValid, failures) as TResponse;
                if (response != null)
                    return response;
            }
        }

        // Continue the pipeline if no validators or no validation errors
        return await next();
    }
}


