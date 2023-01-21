using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spents.Application.InputModels;
using System.Net;
using System.Threading.Tasks;

namespace Spents.IntegrationTests.Tests
{
    [TestClass]
    public class ReceiptTests : BaseTests<ReceiptInputModel>
    {
        private readonly Fixture fixture = new();

        [TestMethod]
        public async Task OnAddAReceipt_ShouldReturnAValidGuid()
        {
            //Arrange
            var receipt = fixture.Create<ReceiptInputModel>();

            //Act
            var response = await PostAsync("/addReceipt", receipt);

            //Assert
            Assert.IsNotNull(response.response);
            Assert.AreEqual(HttpStatusCode.Created, response.statusCode);
        }
    }
}
