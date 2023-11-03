using AutoFixture;
using SpendManagement.Application.Validators;
using SpendManagement.Application.Commands.Receipt.InputModels;
using FluentAssertions;
using System.Linq;

namespace SpendManagement.Unit.Tests.Validators.Receipt
{
    public class ReceiptValidatorTests
    {
        private readonly Fixture fixture = new();
        private readonly AddReceiptValidator receiptValidator;
        private readonly ReceiptItemsValidator receiptItemsValidator;

        public ReceiptValidatorTests()
        {
            receiptValidator = new AddReceiptValidator();
            receiptItemsValidator = new ReceiptItemsValidator();
        }

        private static TheoryData<DateTime?> ReceiptDates()
        {
            return new TheoryData<DateTime?>
            {
                null,
                DateTime.MinValue
            };
        }

        private static TheoryData<IEnumerable<ReceiptItemInputModel?>> ReceiptItems()
        {
            return new TheoryData<IEnumerable<ReceiptItemInputModel?>>
            {
                null!,
                Enumerable.Empty<ReceiptItemInputModel>()
            };
        }

        [Fact]
        public void OnGivenAValidReceipt_ShouldBeValidated()
        {
            //Arrange
            var receiptModel = fixture.Create<ReceiptInputModel>();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(true);
        }

        [Fact]
        public void OnGivenAValidReceipt_WithOutEstablishmentName_Should_Not_BeValidated()
        {
            //Arrange
            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .Without(x => x.EstablishmentName)
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.EstablishmentNameError);
        }

        [Theory]
        [MemberData(nameof(ReceiptDates))]
        public void OnGivenAValidReceipt_ReceiptDateInvalid_Should_Not_BeValidated(DateTime? dateTime)
        {
            //Arrange
            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptDate, dateTime)
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.ReceiptDateMinValueError);
        }

        [Theory]
        [MemberData(nameof(ReceiptItems))]
        public void OnGivenAValidReceipt_ReceiptItemsInvalid_Should_Not_BeValidated(IEnumerable<ReceiptItemInputModel>? receiptItems)
        {
            //Arrange
            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receiptItems)
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.ReceiptItemsError);
        }

        [Fact]
        public void OnGivenAValidReceiptItem_ShouldBeValidated()
        {
            //Arrange
            var receiptModel = fixture.Create<ReceiptInputModel>();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            Assert.True(result.IsValid);
        }
    }
}
