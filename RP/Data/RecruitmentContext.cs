using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RP.Data
{
    public class RecruitmentContext : IdentityDbContext
    {
        public RecruitmentContext(DbContextOptions<RecruitmentContext> options)
            : base(options)
        {
        }
        public DbSet<Job> Job { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<Application> Application { get; set; }
    }
}
