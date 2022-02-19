using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RP.Data;
using RP.Models;

namespace RP.Pages.Jobs
{
    public class EditModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly UserManager<IdentityUser> userManager;

        public EditModel(RecruitmentContext database, UserManager<IdentityUser> userManager)
        {
            this.database = database;
            this.userManager = userManager;
        }
        [BindProperty]
        public Job FormJob { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            FormJob = await database.Job.SingleAsync(j => j.ID == id);
            var user = await userManager.GetUserAsync(User);
            if (FormJob.UserID != user.Id)
            {
                return Forbid();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var dbJob = await database.Job.FindAsync(id);

            var user = await userManager.GetUserAsync(User);
            if (dbJob.UserID != user.Id)
            {
                return Forbid();
            }

            dbJob.Position = FormJob.Position;
            dbJob.Department = FormJob.Department;
            dbJob.JobLocation = FormJob.JobLocation;
            dbJob.Salary = FormJob.Salary;
            dbJob.ContractTerm = FormJob.ContractTerm;


            await database.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
