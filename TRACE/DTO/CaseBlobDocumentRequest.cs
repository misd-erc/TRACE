namespace TRACE.DTO
{
    public class CaseBlobDocumentRequest
    {
        public IFormFile[] Files { get; set; }
        public string CaseNumber { get; set; }
    }
}
