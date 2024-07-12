using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finance_WebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}