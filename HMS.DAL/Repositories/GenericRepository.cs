using HMS.Core.Models.Common;
using HMS.Core.Repositories;
using HMS.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DAL.Repositories
{
    public class GenericRepository<T>(AppDbContext _context) : IGenericRepository<T> where T : BaseEntity
    {
        public DbSet<T> Table =>_context.Set<T>();
        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry=await Table.AddAsync(entity);
            return entityEntry.State==EntityState.Added;
        } 

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await Table.AddRangeAsync(entities);
            return true;
        }

        public IQueryable<T> GetAll()
            => Table;

        public async Task<T> GetByIdAsync(Guid id)
            =>await Table.FirstOrDefaultAsync(x=>x.Id==id);

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression)
            =>Table.Where(expression);

        public async Task<bool> RemoveAsync(Guid id)
        {
            T model=await Table.FirstOrDefaultAsync(x => x.Id == id);
            return Remove(model);
        }

        public bool Remove(T entity)
        {
            EntityEntry<T> entityEntry=Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<int> SaveAsync()
            =>await _context.SaveChangesAsync();

        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry=Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
