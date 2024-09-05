using System.Net.Http;
using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Receipts.Services
{
    public interface ISentBusinessReceiptLoggingService
    {
        HttpResponseMessage LogSentReceiptResponse(Receipt receipt, HttpResponseMessage response, string fileName);
    }
}