using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotNetMentorship.TestAPI.Repository
{
    public class GenericRepository<T> : IUkrainianRepository
    {
        protected UkrainianDbContext _dbContext;

        public GenericRepository(
            UkrainianDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public virtual async Task<Ukrainian> GetByIdAsync(int id)
        {
            return await _dbContext.Ukrainians.FindAsync(id);
        }

        public virtual async Task<Ukrainian> AddAsync(Ukrainian user)
        {
            await _dbContext.Ukrainians.AddAsync(user);
            return user;
        }

        public virtual Task<IEnumerable<Ukrainian>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<Ukrainian> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Ukrainian> UpdateAsync(int id, Ukrainian user)
        {
            throw new NotImplementedException();
        }
    }
}
