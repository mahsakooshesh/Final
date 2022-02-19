using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RP.Models
{
    public class Company : IValidatableObject
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
        [Required]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(
                    "Field cannot be empty",
                    new[] { nameof(Name) }
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
    }
}
