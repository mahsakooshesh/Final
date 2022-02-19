using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RP.Data;
using RP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RecruitmentContext database;
        private readonly AccessControl accessControl;

        public IndexModel(ILogger<IndexModel> logger, RecruitmentContext database, AccessControl accessControl)
        {
            _logger = logger;
            this.database = database;
            this.accessControl = accessControl;
        }
        public IList<Company> Companies { get; set; }
        public IList<Applicant> Applicants { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Companies = await database.Company.ToListAsync();
            Applicants = await database.Applicant.ToListAsync();
            var isApplicant = Applicants.Any(item => item.UserID == accessControl.LoggedInUserID);
            var isCompany = Companies.Any(item => item.UserID == accessControl.LoggedInUserID);
            if(isCompany == true)
            {
                return RedirectToPage("/Companies/index");
            }
            else if (isApplicant == true)
            {
                return RedirectToPage("/Applicants/index");
            }
            else
            {
                return Page();
            }

            
        }
    }
}
