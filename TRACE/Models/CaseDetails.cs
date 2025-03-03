namespace TRACE.Models
{
    public class CaseDetails
    {
        public long ERCCaseID { get; set; }
        public string CaseTitle { get; set; }
        public string CaseNumber { get; set; }
        public string CaseCategory { get; set; }
        public string CaseNature { get; set; }
        public string CaseStatus { get; set; }
        public bool HasPrayedForPA { get; set; }
        public DateTime DateFiled { get; set; }
        public string Complainant { get; set; }
        public string Respondent { get; set; }
    }
}
