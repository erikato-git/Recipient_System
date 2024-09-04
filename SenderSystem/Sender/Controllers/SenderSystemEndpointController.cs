using Microsoft.AspNetCore.Mvc;
using SenderSystem.UtilityLibrary.Receipts.Models;
using SenderSystem.UtilityLibrary.Receipts.Services;

namespace Digst.DigitalPost.Systems.RestPush.Sender.Controllers
{
    [Route("sender-system")]
    [ApiController]
    public class SenderSystemEndpointController
    {
        private readonly IReceivedBusinessReceiptLoggingService logService;

        public SenderSystemEndpointController(IReceivedBusinessReceiptLoggingService logService)
        {
            this.logService = logService;
        }

        [Route("receipt")]
        [HttpPost]
        public IActionResult receipt([FromBody] Receipt businessReceipt)
        {
            logService.HandleReceivedBusinessReceipt(businessReceipt);
            return new OkResult();
        }
    }
}