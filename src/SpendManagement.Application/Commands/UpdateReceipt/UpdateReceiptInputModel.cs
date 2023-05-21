using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using SpendManagement.Application.InputModels.Common;

namespace SpendManagement.Application.Commands.UpdateReceipt
{
    public class UpdateReceiptInputModel
    {
        [FromRoute]
        public Guid Id { get; set; }

        [FromBody]
        public JsonPatchDocument<ReceiptInputModel> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<ReceiptInputModel>();
    }
}
