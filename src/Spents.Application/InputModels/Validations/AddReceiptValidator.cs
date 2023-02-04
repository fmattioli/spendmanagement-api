using FluentValidation;

namespace Spents.Application.InputModels.Validations
{
    public class AddReceiptValidator : AbstractValidator<ReceiptInputModel>
    {
        public AddReceiptValidator()
        {
            RuleFor(x => x.EstablishmentName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Receipt name cannot be null");

            RuleFor(x => x.ReceiptDate)
                .NotNull()
                .NotEmpty()
                .WithMessage("ReceiptDate cannot be null");

            RuleFor(x => x.ReceiptItemsDetail)
                .NotNull()
                .WithMessage("Receipt items cannot be null");

            RuleForEach(x => x.ReceiptItemsDetail).SetValidator(new ReceiptItemsValidator());
        }
    }

    public class ReceiptItemsValidator : AbstractValidator<ReceiptItemsDetailInputModel>
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
        }
    }
}
