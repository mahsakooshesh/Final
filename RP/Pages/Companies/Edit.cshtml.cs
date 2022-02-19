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

namespace RP.Pages.Companies
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
        public Company FormCompany { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            FormCompany = await database.Company.SingleAsync(c => c.ID == id);
            var user = await userManager.GetUserAsync(User);
            if (FormCompany.UserID != user.Id)
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
            var dbCompany = await database.Company.FindAsync(id);

            var user = await userManager.GetUserAsync(User);
            if (dbCompany.UserID != user.Id)
            {
                return Forbid();
            }

            dbCompany.Name = FormCompany.Name;
            dbCompany.Location = FormCompany.Location;


            await database.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
