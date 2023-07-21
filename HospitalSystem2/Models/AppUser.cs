using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HospitalSystem2.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsDeactive { get; set; }
           
    }
}
