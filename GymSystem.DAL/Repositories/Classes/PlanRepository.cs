using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Plan>> GetAll(
            bool isTracked,
            CancellationToken ct = default)
        {
            var plans = isTracked
                ? dbContext.Plans
                : dbContext.Plans.AsNoTracking();

            return await plans.ToListAsync(ct);
        }

        public async Task<Plan?> GetById(
            int id,
            CancellationToken ct = default)
        {
            return await dbContext.Plans
                .FindAsync(new object[] { id }, ct);
        }

        public void Add(Plan plan)
        {
            dbContext.Plans.Add(plan);
        }

        public void Update(Plan plan)
        {
            dbContext.Plans.Update(plan);
        }

        public async Task Delete(
            int id,
            CancellationToken ct = default)
        {
            var plan = await dbContext.Plans
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (plan != null)
                dbContext.Plans.Remove(plan);
        }

        public async Task<int> CompleteAsync(
            CancellationToken ct = default)
        {
            return await dbContext.SaveChangesAsync(ct);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}