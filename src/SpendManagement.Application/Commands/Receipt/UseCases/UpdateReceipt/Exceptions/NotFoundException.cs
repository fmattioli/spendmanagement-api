namespace SpendManagement.Application.Commands.Receipt.UpdateReceipt.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message)
        {
            Data.Add(nameof(NotFoundException), message);
        }

        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public NotFoundException() : base()
        {
        }
    }
}
