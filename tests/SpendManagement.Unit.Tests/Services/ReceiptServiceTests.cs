using AutoFixture;
using FluentAssertions;
using Moq;
using SpendManagement.Application.Commands.Receipt.InputModels;
using SpendManagement.Application.Services;
using SpendManagement.Client.SpendManagementReadModel;
using SpendManagement.WebContracts.Category;
using SpendManagement.WebContracts.Common;

using Web.Contracts.Exceptions;

namespace SpendManagement.Unit.Tests.Services
{
    public class ReceiptServiceTests
    {
        private readonly Mock<ISpendManagementReadModelClient> _spendManagementReadModelClientMock = new();
        private readonly Fixture _fixture = new();
        private readonly ReceiptService _receiptService;

        public ReceiptServiceTests()
        {
            _receiptService = new ReceiptService(_spendManagementReadModelClientMock.Object);
        }

        [Fact]
        public async Task ValidateIfCategoryExistAsync_CategoryExists_ReturnsTrue()
        {
            // Arrange
            var categoryId = _fixture.Create<Guid>();
            var categoryResponse = _fixture.Create<PagedResult<CategoryResponse>>();

            _spendManagementReadModelClientMock
                .Setup(x => x.GetCategoriesAsync(categoryId))
                .ReturnsAsync(categoryResponse);

            // Act
            await _receiptService.ValidateIfCategoryExistAsync(categoryId);

            // Assert
            _spendManagementReadModelClientMock
                .Verify(x => x.GetCategoriesAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task ValidateIfCategoryExistAsync_CategoryDoesNotExist_ThrowsException()
        {
            // Arrange
            var categoryId = _fixture.Create<Guid>();

            _spendManagementReadModelClientMock
                .Setup(x => x.GetCategoriesAsync(categoryId))
                .ThrowsAsync(new NotFoundException("Category not found"));

            // Act + Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _receiptService.ValidateIfCategoryExistAsync(categoryId));
        }

        [Fact]
        public void CalculateReceiptTotals_MakingDiscountsByReceiptItem_Should_ReturnsCorrectTotals()
        {
            // Arrange
            var receiptItems = _fixture
                .Build<ReceiptItemInputModel>()
                .With(x => x.Quantity, 2)
                .With(x => x.ItemPrice, 10)
                .With(x => x.ItemDiscount, 5)
                .CreateMany(1);

            var receiptInputModel = _fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receiptItems)
                .Create();

            // Act
            var result = _receiptService.CalculateReceiptTotals(receiptInputModel);
            
            // Assert
            result.ReceiptItems.Sum(x => x.TotalPrice)
                .Should()
                .Be(15M);

            result.Total
                .Should()
                .Be(15M);

            result.Discount
                .Should()
                .Be(5M);
        }

        [Fact]
        public void CalculateReceiptTotals_MakingDiscountsByReceipt_Should_ReturnsCorrectTotals()
        {
            // Arrange
            var receiptItems = _fixture
                .Build<ReceiptItemInputModel>()
                .With(x => x.Quantity, 2)
                .With(x => x.ItemPrice, 10)
                .With(x => x.ItemDiscount, 0)
                .CreateMany(1);

            var receiptInputModel = _fixture
                .Build<ReceiptInputModel>()
                .With(x => x.ReceiptItems, receiptItems)
                .With(x => x.Discount, 10)
                .Create();

            // Act
            var result = _receiptService.CalculateReceiptTotals(receiptInputModel);

            // Assert
            result.ReceiptItems.Sum(x => x.TotalPrice)
                .Should()
                .Be(20M);

            result.Discount
                .Should()
                .Be(10M);


            result.Total
                .Should()
                .Be(10M);
        }
    }
}
