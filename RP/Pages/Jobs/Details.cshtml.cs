using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RP.Data;
using RP.Models;

namespace RP.Pages.Jobs
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly AccessControl accessControl;

        public DetailsModel(RecruitmentContext database, AccessControl accessControl)
        {
            this.database = database;
            this.accessControl = accessControl;
        }

        public IList<Application> Applications { get; set; }
        [FromQuery]
        public string SearchTerm { get; set; }

        private const string idColumn = "ID";
        private const string educationColumn = "Education";
        private const string experienceLevelColumn = "Experience Level";
        private const string salaryExpectationColumn = "Salary Expectation";
        private const string assessmentScoreColumn = "Assessment Score";
        private string[] sortColumns = { idColumn, educationColumn, experienceLevelColumn, salaryExpectationColumn,assessmentScoreColumn };
        [FromQuery]
        public string SortColumn { get; set; }
        public SelectList SortColumnList { get; set; }

        public async Task OnGetAsync()
        {
            SortColumnList = new SelectList(sortColumns);

            var dbApplication = database.Application.Include(j => j.Job).Include(a => a.Applicant);

            // Start by filtering for only contacts belonging to the logged-in user.
            var query = dbApplication.Where(j => j.Job.User.Id == accessControl.LoggedInUserID).AsNoTracking();

            if (SearchTerm != null)
            {
                query = query.Where(a =>
                    a.Applicant.ID.ToString().Contains(SearchTerm.ToLower()) ||
                    a.Applicant.Education.ToLower().Contains(SearchTerm.ToLower()) ||
                    a.Applicant.ExperienceLevel.ToLower().Contains(SearchTerm.ToLower()) ||
                    a.Applicant.SalaryExpectation.ToString().Contains(SearchTerm.ToLower()) ||
                    a.Applicant.AssessmentScore.ToString().Contains(SearchTerm.ToLower())
                );
            }

            if (SortColumn != null)
            {
                if (SortColumn == idColumn)
                {
                    query = query.OrderBy(j => j.Applicant.ID);
                }
                else if (SortColumn == educationColumn)
                {
                    query = query.OrderBy(j => j.Applicant.Education);
                }
                else if (SortColumn == experienceLevelColumn)
                {
                    query = query.OrderBy(j => j.Applicant.ExperienceLevel);
                }
                else if (SortColumn == salaryExpectationColumn)
                {
                    query = query.OrderBy(j => j.Applicant.SalaryExpectation);
                }
                else if (SortColumn == assessmentScoreColumn)
                {
                    query = query.OrderBy(j => j.Applicant.AssessmentScore);
                }
            }

            Applications = await query.ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        { 
            //find the application that match the ID sent by get request
            var query = await database.Application.FirstOrDefaultAsync(a => a.ID == id);

            string message = "You are being selected";

            query.StatusMessage = message;
            await database.SaveChangesAsync();

            return RedirectToPage("./Index");
        }



    }
}
