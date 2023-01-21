using MediatR;
using Spents.Application.InputModels;

namespace Spents.Application.Queries.GetReceipts
{
    public class GetReceiptsQueryHandler : IRequestHandler<GetReceiptsQuery, IReadOnlyCollection<ReceiptInputModel>>
    {
        public Task<IReadOnlyCollection<ReceiptInputModel>> Handle(GetReceiptsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
