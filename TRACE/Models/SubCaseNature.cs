using System.ComponentModel.DataAnnotations;

namespace TRACE.Models
{
    public class SubCaseNature
    {
        public int SubNatureId { get; set; }

        [Display(Name = "Sub Case Nature Name")]
        public string SubNatureName { get; set; } = null!;

        public string? Description { get; set; }

        [Display(Name = "Choose Case Nature")]
        public long CaseNatureId { get; set; }

        public bool? IsInternal { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual CaseNature CaseNature { get; set; } = null!;
    }
}
