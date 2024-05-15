using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectManagment.Data;

namespace ProjectManagment.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ICollection<Project> Projects { get; set; }
    public ICollection<Issue> Issues { get; set; }
    public ICollection<Comment> Comments { get; set; }
    //public ICollection<Issue> AssignedIssues { get; set; }
}

