using FluentValidation;
using SpendManagement.Application.Commands.Receipt.RecurringReceipt.InputModel;

namespace SpendManagement.Application.Validators
{
    public class AddRecurringReceiptValidator : AbstractValidator<RecurringReceiptInputModel>
    {
        public AddRecurringReceiptValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidationsErrorsMessages.CategoryIdNameError);

            RuleFor(x => x.EstablishmentName)
               .NotNull()
               .NotEmpty()
               .WithMessage(ValidationsErrorsMessages.EstablishmentNameError);

            RuleFor(x => x.DateInitialRecurrence)
                .Must(x => x != DateTime.MinValue)
                .WithMessage(ValidationsErrorsMessages.ReceiptDateMinValueError);

            RuleFor(x => x.RecurrenceTotalPrice)
                .Must(x => x != 0.0M)
                .WithMessage(ValidationsErrorsMessages.RecurrenceTotalPriceInvalid);

            RuleFor(x => x)
               .Must(x => x.DateInitialRecurrence <= x.DateEndRecurrence)
               .WithMessage(ValidationsErrorsMessages.RecurrenceDateIntervalInvalid);
        }
    }
}
