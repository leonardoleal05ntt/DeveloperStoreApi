using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DefaultContext _context;

        public BranchRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default)
        {
            await _context.Branches.AddAsync(branch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return branch;
        }

        public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Branches.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<Branch>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, bool? active = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Branch> query = _context.Branches;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search));
            }

            if (active.HasValue)
                query = query.Where(c => c.Active == active.Value);

            int totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Branch>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
