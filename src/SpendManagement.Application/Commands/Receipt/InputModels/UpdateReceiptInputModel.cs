using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using SpendManagement.WebContracts.Receipt;

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
