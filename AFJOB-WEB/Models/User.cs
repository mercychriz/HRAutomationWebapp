using Microsoft.AspNetCore.Identity;
using System;

namespace AFJOB_WEB.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleID { get; set; }  // This is for RoleId (either JobSeeker or Recruiter)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
