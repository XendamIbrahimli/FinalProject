﻿using HMS.Core.Models;
using HMS.Core.Repositories;
using HMS.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DAL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
