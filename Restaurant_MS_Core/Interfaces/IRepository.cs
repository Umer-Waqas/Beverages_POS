using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    // -------------------------------
    // Querying
    // -------------------------------
    IQueryable<T> Query();
    IQueryable<T> Where(Expression<Func<T, bool>> predicate);

    T? FirstOrDefault(Expression<Func<T, bool>> predicate);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    IEnumerable<T> GetAll();
    Task<List<T>> GetAllAsync();

    T? GetById(object id);
    Task<T?> GetByIdAsync(object id);

    // -------------------------------
    // Any()
    // -------------------------------
    bool Any();
    bool Any(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    // -------------------------------
    // Insert / Update / Delete
    // -------------------------------
    T Insert(T entity);
    Task InsertAsync(T entity);

    void InsertRange(IEnumerable<T> entities);
    Task InsertRangeAsync(IEnumerable<T> entities);

    void Update(T entity);

    void Delete(object id);
    void Delete(T entity);

    // -------------------------------
    // Include (Navigation Properties)
    // -------------------------------
    IQueryable<T> Include(params Expression<Func<T, object>>[] includes);

    // -------------------------------
    // Save
    // -------------------------------
    int Save();
    Task<int> SaveAsync();
}
