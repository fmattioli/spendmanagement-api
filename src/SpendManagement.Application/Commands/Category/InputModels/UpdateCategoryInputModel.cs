using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using SpendManagement.WebContracts.Category;

namespace SpendManagement.Application.Commands.Category.InputModels
{
    public class UpdateCategoryInputModel
    {
        [FromRoute]
        public Guid Id { get; set; }

        [FromBody]
        public JsonPatchDocument<CategoryResponse> CategoryPatchDocument { get; set; } = new JsonPatchDocument<CategoryResponse>();
    }
}
