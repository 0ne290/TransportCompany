using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserDao : IDisposable, IAsyncDisposable
{
    Task<User?> GetByLogin(string login);
}