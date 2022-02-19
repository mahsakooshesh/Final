using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RP.Models
{
    public class Applicant : IValidatableObject
    {
        public int ID { get; set; }

        [Required]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Education { get; set; }
        public string ExperienceLevel { get; set; }
        [Display(Name = "Salary Expectation")]
        public int SalaryExpectation { get; set; }
        public string Location { get; set; }
        [Required]
        public int AssessmentScore { get; set; }
        public bool TookQuiz { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(FName))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(FName) }
                    );
            }
            else if (String.IsNullOrEmpty(LName))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(LName) }
                    );
            }
            else if (String.IsNullOrEmpty(Education))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(Education) }
                    );
            }
            else if (String.IsNullOrEmpty(ExperienceLevel))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(ExperienceLevel) }
                    );
            }
            else if (String.IsNullOrEmpty(Location))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(Location) }
                    );
            }
        }

        public class Assessment
        {
            public string Q1 { get; set; }
            public string Q2 { get; set; }
            public string Q3 { get; set; }

        }
    }
}
