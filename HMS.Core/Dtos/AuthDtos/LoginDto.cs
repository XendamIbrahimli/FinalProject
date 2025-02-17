using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Dtos.AuthDtos
{
    public class LoginDto
    {
        [MaxLength(64)]
        public string UserNameOrEmail { get; set; } = null!;
        [MaxLength(32),DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
