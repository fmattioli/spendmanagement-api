using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spents.Application.InputModels;
using Spents.Core.Entities;

using System;
using System.Net;
using System.Threading.Tasks;

namespace Spents.IntegrationTests.Tests
{
    [TestClass]
    public class ReceiptTests : BaseTests<AddReceiptInputModel>
    {
        private readonly Fixture fixture = new();

        [TestMethod]
        public async Task OnAddAReceipt_ShouldReturnAValidGuid()
        {
            //Arrange
            var receipt = fixture.Create<AddReceiptInputModel>();

            //Act
            var response = await PostAsync("/addReceipt", receipt);

            //Assert
            Assert.IsNotNull(response.response);
            Assert.AreEqual(HttpStatusCode.Created, response.statusCode);
        }
    }
}
