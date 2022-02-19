using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RP.Data;
using RP.Models;

namespace RP.Pages.Jobs
{
    public class IndexModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly AccessControl accessControl;

        public IndexModel(RecruitmentContext database, AccessControl accessControl)
        {
            this.database = database;
            this.accessControl = accessControl;
        }

        public IList<Job> Jobs { get; set; }
        [FromQuery]
        public string SearchTerm { get; set; }

        private const string positionColumn = "Position";
        private const string departmentColumn = "Department";
        private const string jobLocationColumn = "Job Location";
        private const string contractTermColumn = "Contract Term";
        private string[] sortColumns = { positionColumn, departmentColumn, jobLocationColumn, contractTermColumn };
        [FromQuery]
        public string SortColumn { get; set; }
        public SelectList SortColumnList { get; set; }

        public async Task OnGetAsync()
        {
            SortColumnList = new SelectList(sortColumns);

            // Start by filtering for only contacts belonging to the logged-in user.
            var query = database.Job.Where(j => j.User.Id == accessControl.LoggedInUserID).AsNoTracking();

            if (SearchTerm != null)
            {
                query = query.Where(j =>
                    j.Position.ToLower().Contains(SearchTerm.ToLower()) ||
                    j.Department.ToLower().Contains(SearchTerm.ToLower()) ||
                    j.JobLocation.ToLower().Contains(SearchTerm.ToLower()) ||
                    j.ContractTerm.ToLower().Contains(SearchTerm.ToLower())
                );
            }

            if (SortColumn != null)
            {
                if (SortColumn == positionColumn)
                {
                    query = query.OrderBy(j => j.Position);
                }
                else if (SortColumn == departmentColumn)
                {
                    query = query.OrderBy(j => j.Department);
                }
                else if (SortColumn == jobLocationColumn)
                {
                    query = query.OrderBy(j => j.JobLocation);
                }
                else if (SortColumn == contractTermColumn)
                {
                    query = query.OrderBy(j => j.ContractTerm);
                }
            }

            Jobs = await query.ToListAsync();
        }
        


    }
}