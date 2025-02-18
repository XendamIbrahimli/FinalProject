using HMS.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Models
{
    public class Appointment:BaseEntity
    {
        public string Status { get; set; } = null!;
        public Guid? DoctorId { get; set; }
        public Guid? PatienceId { get; set; }
        public Doctor? Doctor { get; set; }
        public Patience? Patience { get; set; }
        public DateTime DateTime { get; set; }

    }
}
