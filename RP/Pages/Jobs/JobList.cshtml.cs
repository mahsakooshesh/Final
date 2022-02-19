using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using GeographyTools;
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
    public class JobListModel : PageModel
    {

        private readonly RecruitmentContext database;

        public JobListModel(RecruitmentContext database)
        {
            this.database = database;
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
            var query = database.Job.AsNoTracking();

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
        GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
        GeoCoordinate coord = null;
        public void OnPost()
        {
            SortColumnList = new SelectList(sortColumns);

            double userLatitude = 0;
            double userLongitude = 0;
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            
            coord = watcher.Position.Location;
            if (coord.IsUnknown != true)
            {
                Console.WriteLine("Lat: {0}, Long: {1}",
                    coord.Latitude,
                    coord.Longitude);
                userLatitude = coord.Latitude;
                userLongitude = coord.Longitude;

            }
            else
            {
                Console.WriteLine("Unknown latitude and longitude.");
                //userLatitude = 58.410809;
                //userLongitude = 15.621373;
            }
            var numberCoord = new GeoCoordinate { Latitude = (double?)userLatitude ?? 0, Longitude = (double?)userLongitude ?? 0 };
            
            var query = database.Job.AsNoTracking();
            List<Job> newJobList = new List<Job>();
            List<double> kmList = new List<double>();

            var nearest = database.Job.Select(x => new GeoCoordinate { Latitude = (double?)x.LatPosition ?? 0, Longitude = (double?)x.LongPosition ?? 0 }).AsEnumerable().OrderBy(x => x.GetDistanceTo(numberCoord)).ToList();
           
            foreach(var near in nearest)
            {
                Coordinate jobCoordinate = new Coordinate();
                jobCoordinate.Longitude = near.Longitude;
                jobCoordinate.Latitude = near.Latitude;

                
                var condition = query.Where(n => n.LatPosition == near.Latitude && n.LongPosition == near.Longitude && !newJobList.Contains(n)).FirstOrDefault();
                newJobList.Add(condition);
                double inKm = near.GetDistanceTo(numberCoord);
                kmList.Add(inKm/1000);
            }

            //show location in range 100 km
            var idx = kmList.Select((x, i) => new { x, i }).Where(x => x.x < 101).Select(x => x.i).ToArray();
            List<Job> newerList = new List<Job>();

            for(int i=0; i<newJobList.Count(); i++)
            {
                if (idx.Contains(i))
                {
                    newerList.Add(newJobList[i]);
                }
            }
            if(idx.Length != 0)
            {
                Jobs = newerList;
            }
            else
            {
                Jobs = newJobList;
            }
           

        }


      



    }
}
