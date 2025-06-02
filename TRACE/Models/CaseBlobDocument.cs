namespace TRACE.Models
{
    public class CaseBlobDocument
    {
        public int DocumentId { get; set; }
        public string? AttachmentName { get; set; }
        public int Ercid { get; set; }
        public string? AttachmentLink { get; set; }
        public DateTime? UploadedAt { get; set; }  
        public string? Module { get; set; }
        public string? Milestone { get; set; }
        public int? DataId { get; set; }

    }
}
