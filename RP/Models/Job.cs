using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RP.Models
{
    public class Job: IValidatableObject
    {
        public int ID { get; set; }

        public string Position { get; set; }
        public double? LongPosition { get; set; }
        public double? LatPosition { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Job Location")]
        public string JobLocation { get; set; }

        [Display(Name = "Salary")]
        public int Salary { get; set; }
        [Required]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }
        public string ContractTerm { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Position))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(Position) }
                    );
            }
            else if (String.IsNullOrEmpty(Department))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(Department) }
                    );
            }
            else if (String.IsNullOrEmpty(JobLocation))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(JobLocation) }
                    );
            }
            else if (String.IsNullOrEmpty(ContractTerm))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(ContractTerm) }
                    );
            }
        }
    }
}
