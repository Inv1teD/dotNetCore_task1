using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DotNetMentorship.TestAPI.Repository
{
    public class UkrainianRepository : GenericRepository<Ukrainian>, IUkrainianRepository
    {
        public UkrainianRepository(UkrainianDbContext context) : base(context) 
        {
            _dbContext = context;
        }

        public override async Task<IEnumerable<Ukrainian>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Ukrainians.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{Repo} All function error ", typeof(UkrainianRepository), $"\n{{Repo}}{ex.Message}");
                return new List<Ukrainian>();
            }
        }

        public override async Task<Ukrainian> AddAsync(Ukrainian user)
        {
            try
            {
                await _dbContext.Ukrainians.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{Repo} Create function error", typeof(UkrainianRepository), $"{{Repo}} {ex.Message}");
                return null;
            }
        }

        public override async Task<Ukrainian> UpdateAsync(int id, Ukrainian user)
        {
            try
            {
                var existingUser = await _dbContext.Ukrainians.Where(x => x.Id == id)
                                                    .FirstOrDefaultAsync();
                    
                    existingUser.Name = user.Name;
                    existingUser.City = user.City;
                    existingUser.IsCalm = user.IsCalm;
                    await _dbContext.SaveChangesAsync();
                    
                    return existingUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{Repo} Update function error", typeof(UkrainianRepository), $"{{Repo}} {ex.Message}");
                return null;
            }
        }

        public override async Task<Ukrainian> DeleteAsync(int id)
        {
            try
            {
                var user = await _dbContext.Ukrainians.Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception();
                }

                _dbContext.Ukrainians.Remove(user);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{Repo} Delete function error", typeof(UkrainianRepository), $"{{Repo}} {ex.Message}");
                return null;
            }
        }
    }
}
