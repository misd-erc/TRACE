﻿using System.ComponentModel.DataAnnotations;

namespace TRACE.Models
{
    public class CaseLastMile
    {
        [Key]
        public string CaseNo { get; set; }
        public string Category { get; set; }
        public string Nature { get; set; }
        public string CaseStatus { get; set; }
        public string LastMilestoneAchieved { get; set; }
    }
}
