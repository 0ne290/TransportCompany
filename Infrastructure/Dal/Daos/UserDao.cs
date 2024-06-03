using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dal.Daos;

public class UserDao(TransportCompanyContext dbContext, ILogger<UserDao> logger) : BaseDao(dbContext), IUserDao
{
    public async Task<User?> GetByLogin(string login) => await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login);
    
    public async Task<bool> Create(User user)
    {
        try
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to add user {@user} to repository", user);
            return false;
        }
    }
}