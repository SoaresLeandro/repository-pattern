using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Data;
using RepositoryPattern.Entities;

namespace RepositoryPattern.Repositories
{
    public class ProductRepository(AppDbContext appDbContext)
    {
        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            await appDbContext.Products.AddAsync(product);
            await appDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            appDbContext.Products.Update(product);
            await appDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> DeleteAsync(Product product, CancellationToken cancellationToken = default)
        {
            appDbContext.Remove(product);
            await appDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await appDbContext.Products.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<Product>> GetAllAsync(int skip = 0, int take = 10)
        {
            return await appDbContext.Products
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
