using FluentValidation;
using MediatR;

namespace Application.Core
{
    /// <summary>
    /// Pipeline behavior for validating requests using FluentValidation before passing them to the next handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="validators">A collection of validators for the request type.</param>
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommandBase
    {
        /// <summary>
        /// The collection of validators for the request.
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        /// <summary>
        /// Handles the validation of the request. If validation fails, throws a <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="request">The incoming request to validate.</param>
        /// <param name="next">The next delegate in the pipeline.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The response from the next handler if validation succeeds.</returns>
        /// <exception cref="ValidationException">Thrown when validation errors are found.</exception>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            var context = new ValidationContext<TRequest>(request);

            var validationFailures = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context, cancellationToken))
            );

            var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(failure => new ValidationError(
                failure.PropertyName,
                failure.ErrorMessage
            )).ToList();

            if (errors.Count != 0)
            {
                throw new ValidationException(errors);
            }

            return await next();
        }
    }
}