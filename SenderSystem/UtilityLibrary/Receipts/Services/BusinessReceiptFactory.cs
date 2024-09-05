using System;
using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Receipts.Services
{
    public class BusinessReceiptFactory : IBusinessReceiptFactory
    {
        public Receipt CreatePositiveBusinessReceipt(string memoUuid)
        {
            Receipt businessReceipt = CreateBusinessReceipt(memoUuid);
            businessReceipt.ReceiptStatus = ReceiptStatus.Completed;

            return businessReceipt;
        }

        public Receipt CreateNegativeBusinessReceipt(string memoUuid = null)
        {
            Receipt businessReceipt = CreateBusinessReceipt(memoUuid);
            businessReceipt.ReceiptStatus = ReceiptStatus.Invalid;

            return businessReceipt;
        }


        public Receipt CreateBusinessReceipt(string fileName = null)
        {
            return new Receipt
            {
                MessageUuid = fileName != null ? new Guid(fileName) : Guid.Empty,
                TransmissionId = Guid.NewGuid(),
                TimeStamp = DateTime.UtcNow
            };
        }
    }
}