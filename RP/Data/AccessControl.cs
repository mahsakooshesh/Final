using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RP.Data
{
    public class AccessControl
    {
        public string LoggedInUserID { get; private set; }

        public AccessControl(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            LoggedInUserID = userManager.GetUserId(httpContextAccessor.HttpContext.User);
        }

        // We can add more methods like this one to control access to different kinds of data.
        public bool UserCanAccess(Applicant applicant)
        {
            return applicant.UserID == LoggedInUserID;
        }
    }
}
