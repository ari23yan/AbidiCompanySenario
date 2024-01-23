namespace AbidiCompanySenario.Data.Interfaces
{
    public interface  IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Remove(T entity);
        void Update(T item);
        Task SaveAsync();
    }
}
