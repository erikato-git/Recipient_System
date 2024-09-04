using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Digst.DigitalPost.SSLClient.Clients;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SenderSystem;
using SenderSystem.UtilityLibrary.Receipts.Configuration;
using SenderSystem.UtilityLibrary.Receipts.Models;
using static System.String;

namespace SenderSystem.UtilityLibrary.Receipts.Sender
{
    public class BusinessReceiptService : IBusinessReceiptService
    {
        private readonly ILogger logger;

        private readonly RecipientSystemConfiguration recipientSystemConfiguration;

        private readonly RestClient restClient;

        public BusinessReceiptService(RestClient restClient, ILogger<BusinessReceiptService> logger)
        {
            this.restClient = restClient;
            this.logger = logger;
            this.recipientSystemConfiguration = recipientSystemConfiguration;
        }

        public Task<HttpResponseMessage> SendRestBusinessReceiptToNgDP(Receipt receipt, string memoUuid)
        {
            Task<HttpResponseMessage> response = restClient.Post(
                Format(recipientSystemConfiguration.ReceiptEndpoint, memoUuid),
                new StringContent(JsonConvert.SerializeObject(receipt), Encoding.UTF8, "application/json"),
                recipientSystemConfiguration.ApiKey);

            if (response.Result.IsSuccessStatusCode)
            {
                logger.LogInformation
                (
                    "Business Receipt with ReceiptStatus: {status} for MeMo with uuid: {Id} sent to NgDP.",
                    (int)receipt.ReceiptStatus,
                    memoUuid
                );
            }
            else
            {
                logger.LogError
                (
                    "Unable to send Business Receipt with ReceiptStatus: {status} for MeMo with uuid: {Id}, httpStatus: {statusCode}, message: {message}",
                    (int)receipt.ReceiptStatus,
                    memoUuid,
                    response.Result.StatusCode,
                    response.Result.Content.ReadAsStringAsync().Result
                );
            }

            return response;
        }
    }
}