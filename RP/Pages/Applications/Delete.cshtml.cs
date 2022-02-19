using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RP.Data;

namespace RP.Pages.Applications
{
    public class DeleteModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly AccessControl accessControl;

        public DeleteModel(RecruitmentContext database, AccessControl accessControl)
        {
            this.database = database;
            this.accessControl = accessControl;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            //find the application that match the ID sent by get request
            var query = await database.Application.Include(a => a.Applicant).FirstOrDefaultAsync(a=> a.ID == id);

            //check the applicant's userID that made the application
            var theApplicantID = query.Applicant.ID;
            var theApplicant = await database.Applicant.FindAsync(theApplicantID);

            if (!accessControl.UserCanAccess(theApplicant))
            {
                return Forbid();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var application = await database.Application.FindAsync(id);

            database.Application.Remove(application);
            await database.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
