using HMS.Core.Models;
using HMS.Core.Repositories;
using HMS.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DAL.Repositories
{
    public class PatienceRepository : GenericRepository<Patience>, IPatienceRepository
    {
        public PatienceRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
