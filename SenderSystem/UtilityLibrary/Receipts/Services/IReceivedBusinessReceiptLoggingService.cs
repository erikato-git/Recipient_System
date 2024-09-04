using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Receipts.Services
{
    public interface IReceivedBusinessReceiptLoggingService
    {
        void HandleReceivedBusinessReceipt(Receipt businessReceipt);
    }
}