using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SenderSystem.UtilityLibrary.Receipts.Models
{
    public class Receipt
    {
        [JsonProperty("transmissionId")]
        [Required]
        public Guid TransmissionId { get; set; }

        [JsonProperty("messageUUID")] public Guid MessageUuid { get; set; }

        [JsonProperty("errorCode")] public string ErrorCode { get; set; }

        [JsonProperty("errorMessage")] public string ErrorMessage { get; set; }

        [JsonProperty("timeStamp")][Required] public DateTime TimeStamp { get; set; }

        [JsonProperty("receiptStatus")]
        [Required]
        public ReceiptStatus ReceiptStatus { get; set; }
    }
}