namespace SenderSystem.UtilityLibrary.Memos.Models
{
    public class MainDocument
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Language { get; set; }

        public string EncodingFormat { get; set; }

        public string FileName { get; set; }

        public string Base64Content { get; set; }
    }
}