using HMS.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Models
{
    public class Patience:BaseEntity
    {
        public string Fullname { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public PatienceComment? PatienceComment { get; set; }
    }
}
