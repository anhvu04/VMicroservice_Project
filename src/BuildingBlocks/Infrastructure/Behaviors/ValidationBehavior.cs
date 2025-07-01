using FluentValidation;
using MediatR;
using Shared.Utils;

namespace Infrastructure.Behaviors;

/// <summary>
/// MediatR pipeline behavior that validates requests before they reach their handlers.
/// Integrates FluentValidation with the Result pattern.
/// </summary>
/// <typeparam name="TRequest">The request type</typeparam>
/// <typeparam name="TResponse">The response type (must be Result or Result&lt;T&gt;)</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // If no validators are registered for this request type, continue to the next behavior/handler
        if (!_validators.Any())
        {
            return await next();
        }

        // Run all validators for this request
        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(request, cancellationToken))
        );

        // Collect all validation failures
        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        // If there are validation failures, return a failure result
        if (failures.Count != 0)
        {
            var errorMessage = string.Join(", ", failures.Select(f => f.ErrorMessage));

            // Handle Result<T> response type
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(Result).GetMethod(nameof(Result.Failure))!.MakeGenericMethod(resultType);
                return (TResponse)failureMethod.Invoke(null, new object[] { errorMessage })!;
            }

            // Handle Result response type
            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(errorMessage);
            }
        }

        // If validation passes, continue to the next behavior/handler
        return await next();
    }
}
