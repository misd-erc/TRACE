namespace TRACE.Models
{
    public class CaseBlobDocument
    {
        public int DocumentID { get; set; }
        public string? AttachmentName { get; set; }
        public int ERCId { get; set; }
        public string? AttachmentLink { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
