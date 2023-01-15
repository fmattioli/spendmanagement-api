using FluentValidation;

namespace Spents.Application.InputModels.Validations
{
    public class AddReceiptValidator : AbstractValidator<AddReceiptInputModel>
    {
        public AddReceiptValidator()
        {
            RuleFor(x => x.ReceiptName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Receipt name cannot be null");

            RuleFor(x => x.ReceiptDate)
                .NotNull()
                .NotEmpty()
                .WithMessage("ReceiptDate cannot be null");

            RuleFor(x => x.ReceiptItems)
                .NotNull()
                .WithMessage("Receipt items cannot be null");

            RuleForEach(x => x.ReceiptItems).SetValidator(new ReceiptItemsValidator());
        }
    }

    public class ReceiptItemsValidator : AbstractValidator<ReceiptItemsDetail>
    {
        public ReceiptItemsValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("The receipt item name  cannot be null or empty");

            RuleFor(x => x.Quantity).Must(x => x >= 1)
                .WithMessage("Please inform at least one quantity. ");

            RuleFor(x => x.Quantity).Must(x => x > 0)
                .WithMessage("Please inform a valid price.");
        }
    }
}
