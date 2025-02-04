using HMS.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Models
{
    public class Department:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<Doctor>? Doctors { get; set; }

    }
}
