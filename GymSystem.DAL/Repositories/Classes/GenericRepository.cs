using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystemG03.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext dbContext;

        public GenericRepository(GymDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public void Add(TEntity item)
        {
            dbContext.Set<TEntity>().Add(item);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity?>().AnyAsync(predicate, ct);  
        }

        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var item =  dbContext.Set<TEntity>().FirstOrDefault(p => p.Id == id);
            if(item != null)
                dbContext.Set<TEntity>().Remove(item);
        }


        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var items = isTracked ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();
           
            return await items.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var items= isTracked ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();
            return await items.ToListAsync();
        }

        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
           var item= await dbContext.Set<TEntity>().FirstOrDefaultAsync(p=> p.Id==id);
            return item;
        }

        public Task GetById()
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity item)
        {
            dbContext.Set<TEntity>().Update(item);
        }
    }
}
