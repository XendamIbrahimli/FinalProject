using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.DoctorDtos
{
    public class DoctorGetDto
    {
        [MaxLength(32)]
        public string? Fullname { get; set; }
        public string? ImageUrl { get; set; }
        [MaxLength(128)]
        public string? FullInfo { get; set; }
        [MaxLength(12)]
        public string? LicenseNumber { get; set; }
        public int ExperienceYear { get; set; }
        public string? DepartmentName { get; set; }
        [MaxLength(32)]
        public string? Username { get; set; }
        [MaxLength(64), EmailAddress]
        public string? Email { get; set; }
        [MaxLength(20), Phone]
        public string? PhoneNumber { get; set; }
        public bool IsApproved { get; set; } 
    }
}
