using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SpendManagement.Application.InputModels.Common;
using Web.Contracts.UseCases.Common;

namespace SpendManagement.Application.Commands.UpdateReceipt
{
    public class UpdateReceiptInputModel
    {
        [FromRoute]
        public Guid Id { get; set; }

        [FromBody]
        public JsonPatchDocument<ReceiptResponse> ReceiptPatchDocument { get; set; } = new JsonPatchDocument<ReceiptResponse>();
    }
}
