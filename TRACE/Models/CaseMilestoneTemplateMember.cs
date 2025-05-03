namespace TRACE.Models
{
    public class CaseMilestoneTemplateMember
    {
        public long CaseMilestoneTemplateId { get; set; }
        public long CaseMilestoneId { get; set; }

        public CaseMilestoneTemplate CaseMilestoneTemplate { get; set; }
        public CaseMilestone CaseMilestone { get; set; }
    }
}
