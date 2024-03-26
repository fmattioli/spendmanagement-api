using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.WebContracts.Receipt;

namespace SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels
{
    public class UpdateVariableReceiptInputModel
    {
        [FromBody]
        public JsonPatchDocument<ReceiptResponse> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<ReceiptResponse>();
    }
}
