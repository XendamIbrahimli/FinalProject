using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        [MaxLength(32)]
        public string DoctorName { get; set; } = null!;
        public DateTime DateTime { get; set; }

    }
}
