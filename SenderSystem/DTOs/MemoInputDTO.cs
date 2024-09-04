using Digst.DigitalPost.Systems.RestPush.Sender.Configuration;
using SenderSystem.UtilityLibrary.Memos.Configuration;
using SenderSystem.UtilityLibrary.Memos.Models;
using SenderSystem.UtilityLibrary.Receipts.Configuration;
using System.ComponentModel.DataAnnotations;

namespace SenderSystem.DTOs
{
    public class MemoInputDTO
    {
        [Required]
        public MeMoHeader Header { get; set; }

        [Required]
        public MeMoBody Body { get; set; }

        [Required]
        public int NumberOfMemos { get; set; }

        // No need to specify that when we send MeMo-message
        public string MemoDirectoryName = "Artifacts";
    }
}
