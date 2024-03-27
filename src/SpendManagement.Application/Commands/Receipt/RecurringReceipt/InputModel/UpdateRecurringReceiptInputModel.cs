using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.WebContracts.Receipt;

namespace SpendManagement.Application.Commands.Receipt.RecurringReceipt.InputModel
{
    public class UpdateRecurringReceiptInputModel
    {
        [FromBody]
        public JsonPatchDocument<RecurringReceiptResponse> RecurringReceiptPatchDocument { get; set; } = new JsonPatchDocument<RecurringReceiptResponse>();
    }
}
