using MediatR;
using SpendManagement.Application.Commands.Receipt.InputModels;

namespace SpendManagement.Application.Commands.Receipt.UseCases.AddReceipt
{
    public class AddReceiptCommand(ReceiptInputModel Receipt) : IRequest<Guid>
    {
        public ReceiptInputModel Receipt { get; set; } = Receipt;
    }
}
