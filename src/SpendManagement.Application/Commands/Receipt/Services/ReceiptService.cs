﻿using SpendManagement.Application.Extensions;
using SpendManagement.Client.SpendManagementReadModel;

namespace SpendManagement.Application.Commands.Receipt.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly ISpendManagementReadModelClient _spendManagementReadModelClient;

        public ReceiptService(ISpendManagementReadModelClient spendManagementReadModelClient)
        {
            _spendManagementReadModelClient = spendManagementReadModelClient;
        }

        public async Task ValidateIfCategoryExistAsync(Guid categoryId)
        {
            await _spendManagementReadModelClient.GetCategoryAsync(categoryId).HandleExceptions("GetCategory");
        }
    }
}
