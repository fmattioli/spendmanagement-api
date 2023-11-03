using FluentValidation;
using SpendManagement.Application.Commands.Receipt.InputModels;

namespace SpendManagement.Application.Validators
{
    public class AddReceiptValidator : AbstractValidator<ReceiptInputModel>
    {
        public AddReceiptValidator()
        {
            RuleFor(x => x.EstablishmentName)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationsErrorsMessages.EstablishmentNameError);

            RuleFor(x => x.ReceiptDate)
                .Must(x => x != DateTime.MinValue)
                .WithMessage(ValidationsErrorsMessages.ReceiptDateMinValueError);

            RuleFor(x => x.ReceiptItems)
                .Must(x => x?.Count() >= 1)
                .WithMessage(ValidationsErrorsMessages.ReceiptItemsError);

            RuleForEach(x => x.ReceiptItems).SetValidator(new ReceiptItemsValidator());
        }
    }

    public class ReceiptItemsValidator : AbstractValidator<ReceiptItemInputModel>
    {
        public ReceiptItemsValidator()
        {
            RuleFor(x => x.ItemName)
                .NotNull()
                .NotEmpty()
                .WithMessage("The receipt item name  cannot be null or empty");

            RuleFor(x => x.ItemPrice).Must(x => x > 0)
                .WithMessage("Please inform at least one quantity. ");

            RuleFor(x => x.Quantity).Must(x => x > 0)
                .WithMessage("Please inform a valid price.");

            RuleFor(x => x.CategoryId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please inform a valid category. ");
        }
    }
}
