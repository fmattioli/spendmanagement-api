using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace SpendManagement.Application.InputModels
{
    public class UpdateReceiptItemInputModel
    {
        [FromRoute]
        public Guid Id { get; set; }

        [FromBody]
        public JsonPatchDocument<AddReceiptItemInputModel> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<AddReceiptItemInputModel>();
    }
}
