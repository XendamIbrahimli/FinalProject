using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.AppointmentDtos
{
    public class UpdateAppointmentDto
    {
        public Guid AppointmentId { get; set; }
        [MaxLength(32)]
        public string NewDoctorName { get; set; } = null!;
        public DateTime NewDateTime { get; set; }
    }
}
