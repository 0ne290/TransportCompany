using Domain.Interfaces;

namespace Application.Interactors;

public class UserInteractor(IUserDao userDao) : IDisposable, IAsyncDisposable
{
    public async Task<bool> Login(string login, string password)
    {
        var user = await userDao.GetByLogin(login);

        return user != null && user.Hash(password) == user.Password;
    }

    public void Dispose()
    {
        userDao.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await userDao.DisposeAsync();
    }
}