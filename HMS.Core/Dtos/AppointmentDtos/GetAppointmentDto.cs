using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.AppointmentDtos
{
    public class GetAppointmentDto
    {
        [MaxLength(32)]
        public string DoctorName { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public string Status { get; set; } = null!;
        public string PatienceName { get; set; } = null!;
    }
}
