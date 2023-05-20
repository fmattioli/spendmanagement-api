using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace SpendManagement.Application.InputModels
{
    public class UpdateReceiptInputModel
    {
        [FromRoute]
        public Guid Id { get; set; }

        [FromBody]
        public JsonPatchDocument<ReceiptInputModel> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<ReceiptInputModel>();
    }
}
