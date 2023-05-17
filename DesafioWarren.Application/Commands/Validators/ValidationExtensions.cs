using FluentValidation;

namespace DesafioWarren.Application.Commands.Validators
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<TObject, TProperty> PropertyMustNotBeNullOrEmpty<TObject, TProperty>(this IRuleBuilderInitial<TObject, TProperty> builderOptions)
        {
            return builderOptions.NotEmpty()
                .NotNull();
        }
        
    }
}