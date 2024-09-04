using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Digst.DigitalPost.SSLClient.Clients;
using Digst.DigitalPost.Systems.RestPush.Sender.Configuration;
using SenderSystem.UtilityLibrary.Receipts.Sender;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SenderSystem;
using SenderSystem.UtilityLibrary.Memos.Services.MemoBuilder;
using SenderSystem.UtilityLibrary.Receipts.Models;

namespace Digst.DigitalPost.Systems.RestPush.Sender.Services
{
    public class MeMoPushService : IMeMoPushService
    {
        private readonly ILogger logger;

        private readonly IMemoBuilder memoBuilder;

        private readonly RestClient restClient;

        private readonly string memoContainerType = "application/x-lzma";

        private readonly string memoFileType = "application/xml";


        private readonly SenderSystemConfiguration senderSystemConfiguration;

        public MeMoPushService(
            ILogger<MeMoPushService> logger,
            RestClient restClient,
            IMemoBuilder memoBuilder,
            SenderSystemConfiguration senderSystemConfiguration
        )
        {
            this.restClient = restClient;
            this.memoBuilder = memoBuilder;
            this.senderSystemConfiguration = senderSystemConfiguration;
            this.logger = logger;
        }


        public void SendMeMo()
        {
            Guid memoUuid = Guid.NewGuid();

            FileStream stream = memoBuilder.CreateMemoFile(memoUuid);

            logger.LogInformation("Sending MeMo to NgDP");

            SendPostRequest(
                new StringContent(new StreamReader(stream).ReadToEnd(), Encoding.UTF8, memoFileType),
                senderSystemConfiguration.NgDPEndpoint + "?memo-message-uuid=" + memoUuid);

            stream.Dispose();
        }

        public void SendMeMoTar()
        {
            FileStream container = memoBuilder.CreateMemoTar();

            logger.LogInformation("Sending MeMo tar to NgDP");

            HttpContent content = new StreamContent(container);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(memoContainerType);

            SendPostRequest(content, senderSystemConfiguration.NgDPEndpoint);

            container.Dispose();
        }

        private void SendPostRequest(HttpContent content, string url)
        {
            Task<HttpResponseMessage> response =
                restClient.Post(url, content, senderSystemConfiguration.ApiKey);
            logger.LogInformation("{}", response.Result.Content.ReadAsStringAsync().Result);

            Receipt receipt =
                JsonConvert.DeserializeObject<Receipt>(response.Result.Content.ReadAsStringAsync().Result);
            logger.LogInformation("Request sent to NgDP. TransmissionId: {}. Timestamp: {}", receipt.TransmissionId,
                receipt.TimeStamp);
        }
    }
}