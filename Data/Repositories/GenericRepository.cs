using AbidiCompanySenario.Data.Context;
using AbidiCompanySenario.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AbidiCompanySenario.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ProjectDbContext Context;
        protected DbSet<T> entities;
        public GenericRepository(ProjectDbContext context)
        {
            Context = context;
            entities = context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await entities.AddAsync(entity);

        }
        public void Remove(T entity)
        {
            entities.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            entities.Update(entity);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
