using System.ComponentModel.DataAnnotations;

namespace TRACE.Models
{
    public class CaseMilestoneTemplateMember
    {
        [Display(Name = "Milestone Template")]
        public long CaseMilestoneTemplateId { get; set; }

        public long CaseMilestoneId { get; set; }

        public CaseMilestoneTemplate CaseMilestoneTemplate { get; set; }
        public CaseMilestone CaseMilestone { get; set; }
    }
}
