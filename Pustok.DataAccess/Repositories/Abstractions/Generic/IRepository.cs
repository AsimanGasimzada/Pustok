using Pustok.Core.Entites.Common;
using System.Linq.Expressions;

namespace Pustok.DataAccess.Repositories.Abstractions.Generic;

public interface IRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    IQueryable<T> GetAll(bool ignoreQueryFilter = false);
    Task<T?> GetByIdAsync(Guid id);
    Task<int> SaveChangesAsync();
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression);
}
