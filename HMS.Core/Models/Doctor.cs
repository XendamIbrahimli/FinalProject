using HMS.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Models
{
    public class Doctor:BaseEntity
    {
        public string Fullname { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string FullInfo { get; set; } = null!;
        public int ExperienceYear { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }

    }
}
