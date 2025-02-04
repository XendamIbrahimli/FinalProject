using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Models
{
    public class User:IdentityUser
    {
        public Doctor? Doctor { get; set; }
        public Patience? Patience { get; set; }
    }
}
