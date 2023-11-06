using AutoFixture;
using Moq;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Commands.Category.UseCases.AddCategory;
using SpendManagement.Application.Producers;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using FluentAssertions;

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
        public async Task Handle_ShouldProduceCategoryCommand()
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
            request.AddCategoryInputModel.Id.Should().Be(result);

            commandProducerMock
                .Verify(
                    x => x.ProduceCommandAsync(It.IsAny<CreateCategoryCommand>()),
                    Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task HandleShouldHandleError()
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
            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>();

            commandProducerMock.Verify(
                x => x.ProduceCommandAsync(It.IsAny<CreateCategoryCommand>()),
                Times.Once);

            commandProducerMock.VerifyNoOtherCalls();
        }
    }
}
