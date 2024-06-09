using Dal.Updaters;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dal.Daos;

// Было бы неплохо превратить все DAO в UnitOfWork и избавиться от повсеместных AsNoTracking(), если получаем данные, которые не идут на дальнейшую выборку/фильтрацию, а сразу идут в работу (хотя, возможно, это имеет смысл только если DbContext'ы долгоживущие)
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
    
    public async Task<bool> Update(string login, User newUser)
    {
        var oldUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
            
        if (oldUser == null)
            return false;
        
        try
        {
            if (oldUser.Login != newUser.Login)
            {
                await dbContext.Users.AddAsync(newUser);
                foreach (var order in oldUser.Orders)
                    order.UserLogin = newUser.Login;
                dbContext.Users.Remove(oldUser);
            }
            else
                UserUpdater.UpdateUser(newUser, oldUser);
            
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to update an old user {@oldUser} to a new one {@newUser} in the repository", oldUser, newUser);
            return false;
        }
    }
}