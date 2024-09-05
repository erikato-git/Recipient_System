using Dk.Digst.Digital.Post.Memolib.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Memos.Services.Logging
{
    public class MemoLoggingService : IMemoLogging
    {
        private readonly ILogger logger;

        public MemoLoggingService(ILogger<MemoLoggingService> logger)
        {
            this.logger = logger;
        }

        public void LogMemoSuccessfullyParsed(Receipt businessReceipt, MessageHeader memoMessageHeader)
        {
            logger.LogInformation
            (
                "Memo received and parsed with id: {Id}, from sender: {Sender}, Positive Business Receipt created with timestamp: {Time}",
                memoMessageHeader.messageUUID,
                memoMessageHeader.Sender.label,
                businessReceipt.TimeStamp
            );

            logger.LogInformation(JsonConvert.SerializeObject(memoMessageHeader, Formatting.Indented));

            logger.LogInformation(JsonConvert.SerializeObject(businessReceipt, Formatting.Indented));
        }

        public void LogMemoUnsuccessfullyParsed(Receipt businessReceipt, string fileName)
        {
            logger.LogInformation
            (
                "Unable to parse MeMo with id: {Id}, Negative Business Receipt created with timestamp: {Time}",
                fileName,
                businessReceipt.TimeStamp
            );
        }
    }
}