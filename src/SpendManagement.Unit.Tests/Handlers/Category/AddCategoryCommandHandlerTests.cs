using AutoFixture;
using Moq;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Commands.Category.UseCases.AddCategory;
using SpendManagement.Application.Producers;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace SpendManagement.Unit.Tests.Handlers.Category
{
    public class AddCategoryCommandHandlerTests
    {
        private readonly AddCategoryCommandHandler handler;
        private readonly Fixture fixture = new();
        private readonly Mock<ICommandProducer> commandProducerMock = new();

        public AddCategoryCommandHandlerTests()
        {
            handler = new(commandProducerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldProduceCategoryCommandAndReturnCategoryId()
        {
            // Arrange
            var categoryInputModel = fixture
                .Create<CategoryInputModel>();

            var request = fixture
                .Build<AddCategoryRequestCommand>()
                .With(x => x.AddCategoryInputModel, categoryInputModel)
                .Create();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(request.AddCategoryInputModel.Id, result);

            commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<CreateCategoryCommand>()),
                    Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_ShouldHandleErrorCase()
        {
            // Arrange
            var categoryInputModel = fixture.Create<CategoryInputModel>();
            var request = fixture.Build<AddCategoryRequestCommand>()
                                 .With(x => x.AddCategoryInputModel, categoryInputModel)
                                 .Create();

            commandProducerMock
                .Setup(x => x.ProduceCommandAsync(It.IsAny<CreateCategoryCommand>()))
                .Throws<Exception>();

            // Act and assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            commandProducerMock.Verify(
                x => x.ProduceCommandAsync(It.IsAny<CreateCategoryCommand>()),
                Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
