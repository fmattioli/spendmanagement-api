using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using SpendManagement.Application.InputModels.Common;

namespace SpendManagement.Application.Commands.UpdateReceiptItem
{
    public class UpdateReceiptItemInputModel
    {
        [FromRoute]
        public Guid Id { get; set; }

        [FromBody]
        public JsonPatchDocument<ReceiptItemInputModel> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<ReceiptItemInputModel>();
    }
}
