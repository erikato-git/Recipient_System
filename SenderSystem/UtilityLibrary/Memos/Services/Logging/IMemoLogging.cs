using Dk.Digst.Digital.Post.Memolib.Model;
using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Memos.Services.Logging
{
    public interface IMemoLogging
    {
        void LogMemoSuccessfullyParsed(Receipt businessReceipt, MessageHeader memoMessageHeader);
        void LogMemoUnsuccessfullyParsed(Receipt businessReceipt, string fileName);
    }
}