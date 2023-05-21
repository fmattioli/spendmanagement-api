using Microsoft.AspNetCore.JsonPatch;

namespace SpendManagement.Application.InputModels
{
    public class UpdateReceiptInputModel
    {
        public Guid Id { get; set; }

        public JsonPatchDocument<ReceiptInputModel> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<ReceiptInputModel>();
    }
}
