using AutoFixture;
using SpendManagement.Application.Validators;
using SpendManagement.Application.Commands.Receipt.InputModels;
using FluentAssertions;

namespace SpendManagement.Unit.Tests.Validators.Receipt
{
    public class ReceiptValidatorTests
    {
        private readonly Fixture fixture = new();
        private readonly AddReceiptValidator receiptValidator;

        public ReceiptValidatorTests()
        {
            receiptValidator = new AddReceiptValidator();
        }

        public static TheoryData<DateTime?> ReceiptDates()
        {
            var teoryData = new TheoryData<DateTime?>
            {
                DateTime.MinValue
            };

            return teoryData;
        }

        public static TheoryData<IEnumerable<ReceiptItemInputModel?>> ReceiptItems()
        {
            var teoryData = new TheoryData<IEnumerable<ReceiptItemInputModel?>>
            {
                Enumerable.Empty<ReceiptItemInputModel>()
            };

            return teoryData;
        }

        [Fact]
        public void OnGivenAValidReceipt_ShouldBeValidated()
        {
            //Arrange
            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .Without(x => x.Discount)
                .Create();

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
        public void OnGivenAValidReceiptItem_WithTwoKindsOfDiscount_ShouldBeNotValidated()
        {
            //Arrange
            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.DiscountFilledOnMoreThanOneField);
        }

        [Fact]
        public void OnGivenAValidReceipt_WithOutReceiptItemName_Should_Not_BeValidated()
        {
            //Arrange
            var receiptItemsModel = fixture
                .Build<ReceiptItemInputModel>()
                .Without(x => x.ItemName)
                .CreateMany();

            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receiptItemsModel)
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.ReceiptItemsItemName);
        }

        [Fact]
        public void OnGivenAValidReceipt_WithOutReceiptItemPrice_Should_Not_BeValidated()
        {
            //Arrange
            var receiptItemsModel = fixture
                .Build<ReceiptItemInputModel>()
                .Without(x => x.ItemPrice)
                .CreateMany();

            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receiptItemsModel)
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.ReceiptItemsItemPrice);
        }

        [Fact]
        public void OnGivenAValidReceipt_WithOutReceiptItemQuantity_Should_Not_BeValidated()
        {
            //Arrange
            var receiptItemsModel = fixture
                .Build<ReceiptItemInputModel>()
                .Without(x => x.Quantity)
                .CreateMany();

            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receiptItemsModel)
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.ReceiptItemsItemQuantity);
        }

        [Fact]
        public void OnGivenAValidReceipt_WithOutReceiptCategoryId_Should_Not_BeValidated()
        {
            //Arrange
            var receiptModel = fixture
                .Build<ReceiptInputModel>()
                .Without(x => x.CategoryId)
                .Create();

            //Act
            var result = this.receiptValidator.Validate(receiptModel);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.CategoryIdNameError);
        }
    }
}
