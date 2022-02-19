using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RP.Data;
using RP.Models;

namespace RP.Pages.Applicants
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly AccessControl accessControl;


        public IndexModel(RecruitmentContext database, AccessControl accessControl)
        {
            this.database = database;
            this.accessControl = accessControl;
        }

        public IList<Applicant> Applicant { get; set; }


        public async Task OnGetAsync()
        {

            var query = database.Applicant.Where(a => a.User.Id == accessControl.LoggedInUserID).AsNoTracking();
            Applicant = await query.ToListAsync();

        }
        
    }
}
