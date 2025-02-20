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
    public class PatienceCommentRepository : GenericRepository<PatienceComment>, IPatienceCommentRepository
    {
        public PatienceCommentRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
