using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RP.Data;
using RP.Models;

namespace RP.Pages.Jobs
{
    public class CreateRandomJobsModel : PageModel
    {
        private readonly RecruitmentContext database;
        private readonly UserManager<IdentityUser> userManager;

        public CreateRandomJobsModel(RecruitmentContext database, UserManager<IdentityUser> userManager)
        {
            this.database = database;
            this.userManager = userManager;
        }

        private Random random;
        private string[] jobLocations;
        private string[] jobPositions;
        private double[] latLocations;
        private double[] longLocations;
        private string[] jobDepartments;
        private string[] contractTerms;
        

        public async Task<IActionResult> OnPostAsync()
        {
            random = new Random();
            IdentityUser user = await userManager.GetUserAsync(User);

            jobLocations = await LoadSingleColumnCSV(@"Data\JobLocations.csv");
            jobPositions = await LoadSingleColumnCSV(@"Data\JobPositions.csv");
            latLocations = await LoadSingleColumnCSVLatitude(@"Data\JobLocations.csv");
            longLocations = await LoadSingleColumnCSVLongitude(@"Data\JobLocations.csv");
            jobDepartments = await LoadSingleColumnCSV(@"Data\JobDepartments.csv");
            contractTerms = await LoadSingleColumnCSV(@"Data\ContractTerms.csv");

            for (int i = 0; i < 5; i++)
            {
                int indexLocation = random.Next(jobLocations.Length);

                string location = jobLocations[indexLocation];
                string position = PickRandomValue(jobPositions);
                double latJobLocation = latLocations[indexLocation];
                double longJobLocation = longLocations[indexLocation];
                string department = PickRandomValue(jobDepartments);
                string contractTerm = PickRandomValue(contractTerms);
                int salary = random.Next(25000, 80000);


                var job = new Job
                {
                    User = user,
                    Position = position,
                    Department = department,
                    JobLocation = location,
                    LatPosition = latJobLocation,
                    LongPosition = longJobLocation,
                    Salary = salary,
                    ContractTerm = contractTerm
                };
                database.Job.Add(job);
            }

            await database.SaveChangesAsync();
            return RedirectToPage("./Index");
        }


        public string PickRandomValue(string[] values)
        {
            int index = random.Next(values.Length);
            return values[index];
        }


        public async Task<string[]> LoadSingleColumnCSV(string path)
        {
            string[] lines = await System.IO.File.ReadAllLinesAsync(path);
            string separator = ",";
            List<string> cities = new List<string>();
            foreach(string line in lines)
            {
                string[] city = line.Split(separator);
                cities.Add(city[0]);

            }

            // Remove empty lines before retuning.
            return cities.Where(line => line.Trim() != "").ToArray();
        }
        public async Task<double[]> LoadSingleColumnCSVLatitude(string path)
        {
            string[] lines = await System.IO.File.ReadAllLinesAsync(path);
            string separator = ",";
            List<double> cities = new List<double>();

            foreach (string line in lines)
            {
                string[] city = line.Split(separator);
                
                double result = double.Parse(city[1], CultureInfo.InvariantCulture);
                cities.Add(result);

            }

            // Remove empty lines before retuning.
            return cities.ToArray();
        }
        public async Task<double[]> LoadSingleColumnCSVLongitude(string path)
        {
            string[] lines = await System.IO.File.ReadAllLinesAsync(path);
            string separator = ",";
            List<double> cities = new List<double>();

            foreach (string line in lines)
            {
                string[] city = line.Split(separator);

                double result = double.Parse(city[2], CultureInfo.InvariantCulture);
                cities.Add(result);

            }

            // Remove empty lines before retuning.
            return cities.ToArray();
        }
    }
}
