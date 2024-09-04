using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SenderSystem.UtilityLibrary.Receipts.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReceiptStatus
    {
        [EnumMember(Value = "COMPLETED")] Completed,

        [EnumMember(Value = "NOT_ALLOWED")] NotAllowed,

        [EnumMember(Value = "INVALID")] Invalid,

        [EnumMember(Value = "RECEIVED")] Received
    }
}