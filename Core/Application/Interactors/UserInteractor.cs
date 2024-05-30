using Domain.Interfaces;

namespace Application.Interactors;

public class UserInteractor(IUserDao userDao) : IDisposable, IAsyncDisposable
{
    public async Task<bool> Login(string login, string password)
    {
        var user = await userDao.GetByLogin(login);

        var x = user.Hash(password);
        Console.WriteLine(x);
        Console.WriteLine(user.Password);

        return user != null && x == user.Password;
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