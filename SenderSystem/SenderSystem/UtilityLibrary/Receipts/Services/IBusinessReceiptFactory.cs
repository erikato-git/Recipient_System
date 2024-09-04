using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Receipts.Services
{
    public interface IBusinessReceiptFactory
    {
        Receipt CreatePositiveBusinessReceipt(string memoUuid);

        Receipt CreateNegativeBusinessReceipt(string memoUuid = null);
    }
}