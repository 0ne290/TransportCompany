using System.Text.Json.Serialization.Metadata;
using Application.Dtos;
using Application.Mappers;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Interactors;

public class UserInteractor(IUserDao userDao, IOrderDao orderDao) : IDisposable, IAsyncDisposable
{
    public async Task<bool> Login(string login, string password)
    {
        var user = await userDao.GetByLogin(login);

        return user != null && user.Hash(password) == user.Password;
    }

    public async Task<bool>
        Registration(string login, string password, string name, string contact, string? defaultAddress) =>
        await userDao.Create(new User
        {
            Login = login, Password = password, Name = name, Contact = contact,
            DefaultAddress = defaultAddress == string.Empty ? null : defaultAddress
        });

    public async Task<IEnumerable<OrderResponseDto>> GetAllOrders(string login)
    {
        var orderMapper = new OrderMapper();
        var orders = await orderDao.GetAllByUserLogin(login);
        
        return orders.Select(o => orderMapper.OrderToOrderResponseDto(o));
    }

    public async Task<string?> GetDefaultAddress(string login)
    {
        var user = await userDao.GetByLogin(login);

        return user?.DefaultAddress;
    }

    public void Dispose()
    {
        userDao.Dispose();
        orderDao.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await userDao.DisposeAsync();
        await orderDao.DisposeAsync();
    }
}
