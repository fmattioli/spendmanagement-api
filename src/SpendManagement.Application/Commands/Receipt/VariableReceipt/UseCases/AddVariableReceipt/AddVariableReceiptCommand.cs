using MediatR;
using SpendManagement.Application.Commands.Receipt.VariableReceipt.InputModels;

namespace SpendManagement.Application.Commands.Receipt.VariableReceipt.UseCases.AddReceipt
{
    public class AddVariableReceiptCommand(ReceiptInputModel Receipt) : IRequest<Guid>
    {
        public ReceiptInputModel Receipt { get; set; } = Receipt;
    }
}
