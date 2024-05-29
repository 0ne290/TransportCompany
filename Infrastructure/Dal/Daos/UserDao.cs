using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Daos;

public class UserDao(TransportCompanyContext dbContext) : IUserDao
{
    public async Task<User?> GetByLogin(string login) => await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login);
    
    public void Dispose()
    {
        dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
    }
}