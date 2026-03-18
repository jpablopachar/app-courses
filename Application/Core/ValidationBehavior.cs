using FluentValidation;
using MediatR;

namespace Application.Core
{
    /// <summary>
    /// Comportamiento de pipeline para validar solicitudes usando FluentValidation antes de pasarlas al siguiente manejador.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de la solicitud.</typeparam>
    /// <typeparam name="TResponse">El tipo de la respuesta.</typeparam>
    /// <param name="validators">Una colección de validadores para el tipo de solicitud.</param>
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommandBase
    {
        /// <summary>
        /// La colección de validadores para la solicitud.
        /// </summary>
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        /// <summary>
        /// Maneja la validación de la solicitud. Si la validación falla, lanza una <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="request">La solicitud entrante a validar.</param>
        /// <param name="next">El siguiente delegado en el pipeline.</param>
        /// <param name="cancellationToken">Un token de cancelación.</param>
        /// <returns>La respuesta del siguiente manejador si la validación es exitosa.</returns>
        /// <exception cref="ValidationException">Se lanza cuando se encuentran errores de validación.</exception>
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