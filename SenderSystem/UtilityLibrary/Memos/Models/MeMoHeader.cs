namespace SenderSystem.UtilityLibrary.Memos.Models
{
    public class MeMoHeader
    {
        public Sender Sender { get; set; }

        public Recipient Recipient { get; set; }

        public string Notification { get; set; }

        public string Label { get; set; }

        public bool Mandatory { get; set; }

        public bool LegalNotification { get; set; }
    }
}