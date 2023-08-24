using Microsoft.AspNetCore.JsonPatch;
using Web.Contracts.Receipt;
using Microsoft.AspNetCore.Mvc;

namespace SpendManagement.Application.Commands.Receipt.InputModels
{
    public class UpdateReceiptInputModel
    {
        [FromRoute]
        public Guid Id { get; set; }

        [FromBody]
        public JsonPatchDocument<ReceiptResponse> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<ReceiptResponse>();
    }
}
