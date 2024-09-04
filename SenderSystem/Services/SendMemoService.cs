using Digst.DigitalPost.SSLClient.Clients;
using Digst.DigitalPost.Systems.RestPush.Sender.Configuration;
using Digst.DigitalPost.Systems.RestPush.Sender.Services;
using SenderSystem.DTOs;
using SenderSystem.UtilityLibrary.Memos.Services.MemoBuilder;

namespace SenderSystem.Services
{
    public class SendMemoService : ISendMemoService
    {
        private MeMoPushService meMoPushService;

        public SendMemoService(ILogger<MeMoPushService> logger, RestClient restClient, IMemoBuilder memoBuilder, SenderSystemConfiguration senderSystemConfiguration)
        {
            meMoPushService = new MeMoPushService(logger, restClient, memoBuilder, senderSystemConfiguration);
        }


        public Task ProcessMemoInput(MemoInputDTO input)
        {
            GlobalStates.memoConfiguration.Header = input.Header;
            GlobalStates.memoConfiguration.Body = input.Body;
            GlobalStates.memoConfiguration.NumberOfMemos = input.NumberOfMemos;
            GlobalStates.memoConfiguration.MemoDirectoryName = input.MemoDirectoryName;

            meMoPushService.SendMeMo();

            return Task.CompletedTask;
        }
    }
}
