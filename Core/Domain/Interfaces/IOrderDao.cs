using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderDao : IDisposable, IAsyncDisposable
{
    Task<IEnumerable<Order>> GetAllByUserLogin(string userLogin);
}