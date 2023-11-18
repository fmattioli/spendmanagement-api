using AutoFixture;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SpendManagement.Application.Commands.Validators;

namespace SpendManagement.Unit.Tests.Validators.Category
{
    public class CategoryValidatorsTests
    {
        private readonly Fixture fixture = new();

        [Fact]
        public void OnGivenAValidJsonPatch_ValidationsMustBeSuccessfully()
        {
            //Arrange
            var validator = new JsonPatchValidator();

            //Act
            var result = validator.Validate(new JsonPatchError(fixture.Create<object>(), fixture.Create<Operation>(), string.Empty));

            //Act
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OnGivenAnInvalidJsonPatch_AnError_ShouldOcurred()
        {
            //Arrange
            var jsonError = fixture.Create<JsonPatchError>();
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
            var jsonError = fixture.Create<JsonPatchError>();
            var validator = new JsonPatchValidator();

            //Act and act
            Assert.Throws<ValidationException>(() => validator.ValidateAndThrow(jsonError));
        }
    }
}
