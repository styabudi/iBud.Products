using System.Linq.Expressions;

namespace iBud.Products.Infrastructure.Interfaces;

public interface IRepositoryBase<T>
{
    IQueryable<T> Include(params Expression<Func<T, Object>>[] includes);
        IQueryable<T> FindAll();
        IQueryable<T> FindAllWithInactive();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T FindById(Guid Id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
}