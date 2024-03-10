using AutoFixture;
using FluentAssertions;
using SpendManagement.Application.Commands.RecurringReceipt.InputModel;
using SpendManagement.Application.Validators;

namespace SpendManagement.Unit.Tests.Validators.RecurringReceipt
{
    public class RecurringReceiptValidatorTests
    {
        private readonly Fixture _fixture = new();
        private readonly AddRecurringReceiptValidator _receiptValidator;
        public RecurringReceiptValidatorTests()
        {
            _receiptValidator = new AddRecurringReceiptValidator();
        }

        [Fact]
        public void OnGivenAnInvalidReceiptRecurring_CategoryIdAsNull_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .Without(x => x.CategoryId)
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.CategoryIdNameError);
        }

        [Fact]
        public void OnGivenAnInvalidReceiptRecurring_CategoryIdEmpty_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.CategoryId, Guid.Empty)
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.CategoryIdNameError);
        }


        [Fact]
        public void OnGivenAnInvalidReceiptRecurring_EstabishmentNameAsNull_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .Without(x => x.EstablishmentName)
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.EstablishmentNameError);
        }

        [Fact]
        public void OnGivenAnInvalidReceiptRecurring_EstabishmentNameEmpty_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.EstablishmentName, string.Empty)
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.EstablishmentNameError);
        }

        [Fact]
        public void OnGivenAnInvalidReceiptRecurring_DateInitialRecurrence_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.DateInitialRecurrence, DateTime.MinValue)
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.ReceiptDateMinValueError);
        }

        [Fact]
        public void OnGivenAnInvalidReceiptRecurring_RecurrenceTotalPrice_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.RecurrenceTotalPrice, 0.0M)
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.RecurrenceTotalPriceInvalid);
        }

        [Fact]
        public void OnGivenAnInvalidReceiptRecurring_RecurrenceDateIntervalInvalid_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.DateInitialRecurrence, DateTime.Now)
                .With(x => x.DateEndRecurrence, DateTime.Now.AddDays(-20))
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.RecurrenceDateIntervalInvalid);
        }

        [Fact]
        public void OnGivenAValidReceiptRecurring_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<RecurringReceiptInputModel>()
                .With(x => x.DateInitialRecurrence, DateTime.Now)
                .With(x => x.DateEndRecurrence, DateTime.Now.AddDays(20))
                .Create();

            //Act
            var result = this._receiptValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(true);
            result.Errors.Should().BeEmpty();
        }
    }
}
