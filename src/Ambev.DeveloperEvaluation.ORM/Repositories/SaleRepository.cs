using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                        .Include(s => s.Customer)
                        .Include(s => s.Branch)
                        .Include(s => s.Items)
                            .ThenInclude(i => i.Product)
                        .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Sales.FirstOrDefaultAsync(o => o.SaleNumber.ToLower().Equals(saleNumber.ToLower()), cancellationToken);
        }

        public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<Sale>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, bool? isCancelled = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Sale> query = _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Branch)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s => s.SaleNumber.Contains(search) ||
                                         s.Customer.Name.Contains(search) ||
                                         s.Branch.Name.Contains(search) ||
                                         s.Items.Any(i => i.Product.Name.Contains(search)));
            }

            if (isCancelled.HasValue)
                query = query.Where(c => c.IsCancelled == isCancelled.Value);

            int totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(s => s.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Sale>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
