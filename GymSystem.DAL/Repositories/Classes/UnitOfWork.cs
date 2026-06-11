using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystemG03.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext dbContext;
        private readonly Dictionary<string,object> _Repos=[];

        public UnitOfWork(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName=typeof(TEntity).Name;//key

            if (_Repos.TryGetValue(TypeName, out object OldRepository))
                return (IGenericRepository<TEntity>)OldRepository;

            var NewRepository = new GenericRepository<TEntity>(dbContext);

            _Repos[TypeName]=NewRepository;

            return NewRepository;
        }
    }
}