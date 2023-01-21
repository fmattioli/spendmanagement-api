using MediatR;
using Spents.Application.InputModels;

namespace Spents.Application.Queries.GetReceipts
{
    public record GetReceiptsQuery(GetReceiptsViewModel FilterReceiptModel) : IRequest<IReadOnlyCollection<ReceiptInputModel>>;
}
