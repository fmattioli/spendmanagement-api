using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SpendManagement.Application.Commands.Category.InputModels;
using SpendManagement.Application.Commands.Validators;
using SpendManagement.Application.Validators;

namespace SpendManagement.Unit.Tests.Validators.Category
{
    public class CategoryValidatorsTests
    {
        private readonly Fixture _fixture = new();

        private readonly AddCategoryValidator _categoryValidator;

        public CategoryValidatorsTests()
        {
            this._categoryValidator = new AddCategoryValidator();
        }

        [Fact]
        public void OnGivenAValidJsonPatch_ValidationsMustBeSuccessfully()
        {
            //Arrange
            var validator = new JsonPatchValidator();

            //Act
            var result = validator.Validate(new JsonPatchError(_fixture.Create<object>(), _fixture.Create<Operation>(), string.Empty));

            //Act
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OnGivenAnInvalidJsonPatch_AnError_ShouldOcurred()
        {
            //Arrange
            var jsonError = _fixture.Create<JsonPatchError>();
            var validator = new JsonPatchValidator();

            //Act
            var result = validator.Validate(jsonError);

            //Act
            Assert.False(result.IsValid);
        }

        [Fact]
        public void OnGivenAnInvalidJsonPatch_AnExceptionShouldOcurred()
        {
            //Arrange
            var jsonError = _fixture.Create<JsonPatchError>();
            var validator = new JsonPatchValidator();

            //Act and act
            Assert.Throws<ValidationException>(() => validator.ValidateAndThrow(jsonError));
        }

        [Fact]
        public void OnGivenAnInvalidCategory_CategoryIdEmpty_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<CategoryInputModel>()
                .With(x => x.Id, Guid.Empty)
                .Create();

            //Act
            var result = this._categoryValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.CategoryIdNameError);
        }

        [Fact]
        public void OnGivenAnInvalidCategory_CategoryNameEmpty_ShouldBeNotValidated()
        {
            //Arrange
            var recurringReceipt = _fixture
                .Build<CategoryInputModel>()
                .With(x => x.Name, string.Empty)
                .Create();

            //Act
            var result = this._categoryValidator.Validate(recurringReceipt);

            //Act
            result.IsValid.Should().Be(false);
            result.Errors.Should().Contain(e => e.ErrorMessage == ValidationsErrorsMessages.CategoryNameError);
        }
    }
}
