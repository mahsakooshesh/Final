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

namespace RP.Pages.Applications
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly UserManager<IdentityUser> userManager;
        public IList<Job> Job { get; set; }
        public IList<Applicant> Applicant { get; set; }
        public IList<Application> Application { get; set; }

        public DetailsModel(RecruitmentContext database, UserManager<IdentityUser> userManager)
        {
            this.database = database;
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {

            var dbJob = database.Job.Where(j => j.ID == id).AsNoTracking();
            Job = await dbJob.ToListAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {

            IdentityUser user = await userManager.GetUserAsync(User);

            var dbJob = database.Job.Where(j => j.ID == id).AsNoTracking();
            Job = await dbJob.ToListAsync();

            var dbApplicant = database.Applicant.Where(a => a.User == user).AsNoTracking();
            Applicant = await dbApplicant.ToListAsync();
            Application theApplication = new Application
            {
                User = user,
                StatusMessage = "Application is being reviewed"
            };

            foreach (var item in Job)
            {
                theApplication.JobID = item.ID;
            }
            foreach (var item in Applicant)
            {
                theApplication.ApplicantID = item.ID;
            }

            var theJobsApplicant = await database.Application.Where(j => j.JobID == theApplication.JobID).ToListAsync();

            //check if the applicant has applied for this job before
            var hasAppliedJob = theJobsApplicant.Any(item => item.ApplicantID == theApplication.ApplicantID);

            if(hasAppliedJob == true)
            {
                return RedirectToPage("ApplyFailed");
            }
            else
            {
                    await database.Application.AddAsync(theApplication);
                    await database.SaveChangesAsync();

                   
            }
            return RedirectToPage("../Applications/Index");
           
        }
    }

}
