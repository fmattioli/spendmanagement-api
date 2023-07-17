﻿namespace SpendManagement.Application.Commands.UpdateReceipt.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message)
        {
            Data.Add(nameof(NotFoundException), message);
        }
    }
}
