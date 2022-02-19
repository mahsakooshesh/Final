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
using static RP.Models.Applicant;

namespace RP.Pages.Applicants
{
    public class QuizModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly AccessControl accessControl;

        public QuizModel(RecruitmentContext database, AccessControl accessControl)
        {
            this.database = database;
            this.accessControl = accessControl;
        }
        [BindProperty]
        public Assessment FormAssessment { get; set; }
        public Applicant Applicant { get; set; }

        private async Task LoadApplicant(int id)
        {
            Applicant = await database.Applicant.SingleAsync(a => a.ID == id);
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            int score = 0;

            if(FormAssessment.Q1.ToLower() == "4")
            {
                score += 1;
            }
            if(FormAssessment.Q2.ToLower() == "yellow")
            {
                score += 1;
            }
            if (FormAssessment.Q3.ToLower() == "stockholm")
            {
                score += 1;
            }

            await LoadApplicant(id);

            Applicant.AssessmentScore = score;
            Applicant.TookQuiz = true;
            await database.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
