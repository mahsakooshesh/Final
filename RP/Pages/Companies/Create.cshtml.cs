using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RP.Data;
using RP.Models;

namespace RP.Pages.Companies
{
    public class CreateModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly UserManager<IdentityUser> userManager;
        public CreateModel(RecruitmentContext database, UserManager<IdentityUser> userManager)
        {
            this.database = database;
            this.userManager = userManager;

        }
        public async Task<IActionResult> OnPostAsync()
        {
            IdentityUser user = await userManager.GetUserAsync(User);
            var emptyProfile = new Company
            {
                User = user,
                Name = "", 
                Location = ""
            };
            await database.Company.AddAsync(emptyProfile);
            await database.SaveChangesAsync();
            return RedirectToPage("./Edit", new { id = emptyProfile.ID });
        }
    }
}
