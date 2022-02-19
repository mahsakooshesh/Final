using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RP.Data;
using RP.Models;

namespace RP.Pages.Companies
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

        public IList<Company> Company { get; set; }

        public async Task OnGetAsync()
        {

            var query = database.Company.Where(c => c.User.Id == accessControl.LoggedInUserID).AsNoTracking();
            Company = await query.ToListAsync();

        }
    }
}
