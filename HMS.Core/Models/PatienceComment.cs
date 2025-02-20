using HMS.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Core.Models
{
    public class PatienceComment:BaseEntity
    {
        public string Comment { get; set; } = null!;
        public string PatienceFullname { get; set; } = null!;
        public Guid? PatienceId { get; set; }
        public Patience? Patience { get; set; }

    }
}
