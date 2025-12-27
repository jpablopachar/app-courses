namespace Application.Core
{
    /// <summary>
    /// Represents a validation error for a specific property.
    /// </summary>
    /// <param name="PropertyName">The name of the property that failed validation.</param>
    /// <param name="ErrorMessage">The error message describing the validation failure.</param>
    public sealed record ValidationError(string PropertyName, string ErrorMessage);
}