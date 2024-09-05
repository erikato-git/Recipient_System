using System.IO;
using Dk.Digst.Digital.Post.Memolib.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SenderSystem.UtilityLibrary.Memos.Services.Logging;
using SenderSystem.UtilityLibrary.Memos.Services.Parser;
using SenderSystem.UtilityLibrary.Receipts.Models;
using SenderSystem.UtilityLibrary.Receipts.Sender;
using SenderSystem.UtilityLibrary.Receipts.Services;

namespace Digst.DigitalPost.Systems.RestPush.Recipient.Controllers
{
    [Route("recipient-system")]
    [ApiController]
    public class RecipientSystemMemoEndpointController : ControllerBase
    {
        private readonly IBusinessReceiptFactory businessReceiptFactory;

        private readonly IBusinessReceiptService businessReceiptService;

        private readonly IMemoLogging memoLoggingService;

        private readonly IMemoService memoService;

        public RecipientSystemMemoEndpointController(
            IMemoLogging memoLoggingSerice,
            IMemoService memoService,
            IBusinessReceiptFactory businessReceipt,
            IBusinessReceiptService businessReceiptService)
        {
            memoLoggingService = memoLoggingSerice;
            this.memoService = memoService;
            businessReceiptFactory = businessReceipt;
            this.businessReceiptService = businessReceiptService;
        }

        [HttpPost]
        public IActionResult RecipientSystemMemoEndpoint([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return StatusCode(400, "MeMo is not present in request");
            }

            Stream memoFile = file.OpenReadStream();
            MessageHeader memoMessageHeader = memoService.ParseMemoHeader(memoFile);

            Receipt receipt;
            if (memoMessageHeader != null)
            {
                receipt = businessReceiptFactory.CreatePositiveBusinessReceipt(file.FileName);
                memoLoggingService.LogMemoSuccessfullyParsed(receipt, memoMessageHeader);
            }
            else
            {
                receipt = businessReceiptFactory.CreateNegativeBusinessReceipt(file.FileName);
                memoLoggingService.LogMemoUnsuccessfullyParsed(receipt, file.FileName);
            }

            businessReceiptService.SendRestBusinessReceiptToNgDP(receipt, file.FileName);

            return Ok(memoMessageHeader != null ? memoMessageHeader.messageUUID : "Unable to parse MeMo");
        }
    }
}