// ReSharper disable PossibleMultipleEnumeration
using FluentValidation;

namespace BankingApp.Application.Core.Extensions;

public static class ApplicationCoreValidationExtensions
{
    public static IRuleBuilder<T, TProperty> MustBeOneOf<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IEnumerable<TProperty> values)
    {
        if (values is null || !values.Any())
        {
            throw new ArgumentException("Values cannot be null or empty", nameof(values));
        }

        var valuesArray = values.ToArray();

        return ruleBuilder.Must(values.Contains)
            .WithMessage($"{{PropertyName}} must be one of {string.Join(", ", valuesArray)}");
    }
}