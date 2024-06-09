using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserDao : IDisposable, IAsyncDisposable
{
    Task<User?> GetByLogin(string login);

    Task<bool> Create(User user);

    Task<bool> Update(string login, User newUser);
}