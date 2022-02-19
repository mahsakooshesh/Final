using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RP.Models
{
    public class Application
    {
        public int ID { get; set; }

        [Required]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }
        [ForeignKey("ApplicantID")]
        public int? ApplicantID { get; set; }
        public Applicant Applicant { get; set; }
        [ForeignKey("JobID")]
        public int? JobID { get; set; }
        public Job Job { get; set; }
        [Display(Name = "Application Status")]
        public string StatusMessage { get; set; }
    }
}
