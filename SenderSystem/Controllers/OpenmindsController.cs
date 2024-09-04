using SenderSystem.UtilityLibrary.Receipts.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SenderSystem.DTOs;
using SenderSystem.Services;
using System.Text.Json;

namespace SenderSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenmindsController : ControllerBase
    {
        private readonly ISendMemoService _sendMemoService;

        public OpenmindsController(ISendMemoService sendMemoService)
        {
            _sendMemoService = sendMemoService;
        }


        [HttpGet("/Dummy")]
        public async Task<String> Dummy()
        {
            return "Dummy";
        }

        [HttpPost("/CreateMemo")]
        public async Task<IActionResult> CreateMemo([FromBody] MemoInputDTO memoDto)
        {
            try
            {
                await _sendMemoService.ProcessMemoInput(memoDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Memo was sent");
        }
    }
}
