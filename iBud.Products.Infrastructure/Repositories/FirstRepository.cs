using iBud.Products.Infrastructure.Context;
using iBud.Products.Infrastructure.Interfaces;
using iBud.Products.Infrastructure.Models.Tests;

namespace iBud.Products.Infrastructure.Repositories;
class FirstRepository : RepositoryBase<First>, IFirstRepository
{
    public FirstRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}