using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
        }

        public IQueryable<T> GetAll(bool noTracking = true)
        {
            var set = _dbSet;
            if (noTracking)
            {
                set.AsNoTracking();
            }
            return set;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void InsertAll(List<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Remove(IEnumerable<T> entitiesToRemove)
        {
            _dbSet.RemoveRange(entitiesToRemove);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _dbSet.Update(entity);
        }
    }
}
