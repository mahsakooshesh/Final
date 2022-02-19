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

namespace RP.Pages.Applicants
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
        public Applicant FormApplicant { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            FormApplicant = await database.Applicant.SingleAsync(a => a.ID == id);
            var user = await userManager.GetUserAsync(User);
            if (FormApplicant.UserID != user.Id)
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
            var dbApplicant = await database.Applicant.FindAsync(id);

            var user = await userManager.GetUserAsync(User);
            if (dbApplicant.UserID != user.Id)
            {
                return Forbid();
            }

            dbApplicant.FName = FormApplicant.FName;
            dbApplicant.LName = FormApplicant.LName;
            dbApplicant.Education = FormApplicant.Education;
            dbApplicant.ExperienceLevel = FormApplicant.ExperienceLevel;
            dbApplicant.SalaryExpectation = FormApplicant.SalaryExpectation;
            dbApplicant.FName = FormApplicant.FName;
            dbApplicant.Location = FormApplicant.Location;


            await database.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
