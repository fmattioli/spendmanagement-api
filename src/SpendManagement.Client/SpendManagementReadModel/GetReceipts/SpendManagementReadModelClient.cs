﻿using Microsoft.Extensions.Configuration;
using SpendManagement.Client.Configuration;

using System.Net.Http.Json;

using Web.Contracts.UseCases.Common;

namespace SpendManagement.Client.SpendManagementReadModel.GetReceipts
{
    public class SpendManagementReadModelClient : BaseClient, ISpendManagementReadModelClient
    {
        public SpendManagementReadModelClient(IApiConfiguration configuration, HttpClient httpClient ) : base(httpClient, configuration)
        {
        }

        public async Task<ReceiptResponse?> GetReceipt(Guid receiptId)
        {
            var queryParams = new Dictionary<string, object>
            {
                 { "Id", receiptId },
            };
            var retorno = await GetAsync<ReceiptResponse>(queryParams);
            return retorno;
        }
    }
}