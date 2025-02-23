using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.BL.Helpers
{
    public static class RoleConstants
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Patience = "Patience";
        public const string Moderator = "Moderator";

        public const string AdminAndModerator = Admin + "," + Moderator;
        public const string AdminModeratorDoctor = Admin + "," + Moderator+","+Doctor;

    }
}
