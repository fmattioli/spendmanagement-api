namespace SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions
{
    public sealed class JsonPatchInvalidException : Exception
    {
        public JsonPatchInvalidException(string message)
        {
            Data.Add(nameof(JsonPatchInvalidException), message);
        }
    }
}
