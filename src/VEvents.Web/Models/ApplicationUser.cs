﻿using Microsoft.AspNetCore.Identity;

namespace VEvents.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
