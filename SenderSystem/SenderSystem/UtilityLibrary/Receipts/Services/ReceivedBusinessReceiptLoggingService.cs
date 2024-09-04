using Microsoft.Extensions.Logging;
using SenderSystem.UtilityLibrary.Receipts.Models;

namespace SenderSystem.UtilityLibrary.Receipts.Services
{
    public class ReceivedBusinessReceiptLoggingService : IReceivedBusinessReceiptLoggingService
    {
        private readonly ILogger logger;

        public ReceivedBusinessReceiptLoggingService(ILogger<ReceivedBusinessReceiptLoggingService> logger)
        {
            this.logger = logger;
        }

        public void HandleReceivedBusinessReceipt(Receipt businessReceipt)
        {
            if (businessReceipt.ReceiptStatus == ReceiptStatus.Completed)
            {
                logger.LogInformation(
                    "Positive Business Receipt received from NgDP for MeMo with uuid: {}, status: {}, timestamp {}",
                    businessReceipt.MessageUuid,
                    businessReceipt.ReceiptStatus,
                    businessReceipt.TimeStamp);
            }
            else
            {
                logger.LogInformation(
                    "Negative Business Receipt received from NgDP for MeMo with uuid: {}, status: {}, errorMessage: {}, errorCode: {}, timestamp {}",
                    businessReceipt.MessageUuid,
                    businessReceipt.ReceiptStatus,
                    businessReceipt.ErrorMessage,
                    businessReceipt.ErrorCode,
                    businessReceipt.TimeStamp);
            }
        }
    }
}