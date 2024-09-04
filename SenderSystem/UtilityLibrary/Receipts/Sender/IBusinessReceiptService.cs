using System.Net.Http;
using System.Threading.Tasks;
using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Receipts.Sender
{
    public interface IBusinessReceiptService
    {
        Task<HttpResponseMessage> SendRestBusinessReceiptToNgDP(Receipt receipt, string memoUuid);
    }
}