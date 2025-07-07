namespace TRACE.Models
{
    public class Intervenor
    {
        public int IntervenorId { get; set; }

        public long? CaseId { get; set; }

        public long? CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
