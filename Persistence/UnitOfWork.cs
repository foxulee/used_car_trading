using System.Threading.Tasks;
using project_vega.Core;

namespace project_vega.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext _context;

        public UnitOfWork(VegaDbContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}