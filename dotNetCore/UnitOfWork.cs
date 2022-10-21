using DotNetMentorship.TestAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace DotNetMentorship.TestAPI
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UkrainianDbContext _context;
        private readonly ILogger _logger;

        public IUkrainianRepository Ukrainians { get; private set; }


        public UnitOfWork(UkrainianDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Ukrainians = new UkrainianRepository(context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
