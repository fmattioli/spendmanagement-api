using FluentValidation;
using SpendManagement.Application.Commands.Category.InputModels;

namespace SpendManagement.Application.Validators
{
    public class AddCategoryValidator : AbstractValidator<CategoryInputModel>
    {
        public AddCategoryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationsErrorsMessages.CategoryIdNameError);

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationsErrorsMessages.CategoryNameError);
        }
    }
}
