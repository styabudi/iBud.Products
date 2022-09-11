using System.Linq.Expressions;
using iBud.Products.Infrastructure.Context;
using iBud.Products.Infrastructure.Interfaces;
using iBud.Products.Infrastructure.Models.Common;
using Microsoft.EntityFrameworkCore;
using static iBud.Products.Infrastructure.Common.Constants.CommonConstant;

namespace iBud.Products.Infrastructure.Repositories;
public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseModel
{
    protected AppDbContext _dbContext;

    public RepositoryBase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<T> Include(params Expression<Func<T, Object>>[] includes)
    {
        IQueryable<T> query = this._dbContext.Set<T>().Include(includes[0]).Where(x => x.IsActive == StatusId.Active);
        foreach (var include in includes.Skip(1))
        {
            query = query.Include(include);
        }
        return query;
    }
    public IQueryable<T> FindAll()
    {

        return this._dbContext.Set<T>().Where(x => x.IsActive == StatusId.Active).AsNoTracking();
    }
    public IQueryable<T> FindAllWithInactive()
    {

        return this._dbContext.Set<T>().AsNoTracking();
    }
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return this._dbContext.Set<T>().Where(expression).Where(x => x.IsActive == StatusId.Active).AsNoTracking();
    }
    public T FindById(Guid Id)
    {
        return this._dbContext.Set<T>().Where(x => x.Id == Id && x.IsActive == StatusId.Active).AsNoTracking().FirstOrDefault();
    }
    public void Create(T entity)
    {
        this._dbContext.Set<T>().Add(entity);
    }
    public void Update(T entity)
    {
        this._dbContext.Set<T>().Update(entity);
    }
    public void Delete(T entity)
    {
        this._dbContext.Set<T>().Remove(entity);
    }

    public void Save(){
        this._dbContext.SaveChanges();
    }
}