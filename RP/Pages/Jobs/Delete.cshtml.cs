using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using RP.Data;

namespace RP.Pages.Jobs
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
            var job = await database.Job.FindAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var job = await database.Job.FindAsync(id);

            database.Job.Remove(job);
            await database.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
