using Application.Dtos;
using Application.Mappers;
using Domain.Interfaces;

namespace Application.Interactors;

public class UserInteractor(IUserDao userDao, IOrderDao orderDao) : IDisposable, IAsyncDisposable
{
    public async Task<bool> Login(string login, string password)
    {
        var user = await userDao.GetByLogin(login);

        return user != null && user.Hash(password) == user.Password;
    }

    public async Task<bool> Registration(UserRequestDto userDto) =>
        await userDao.Create(UserMapper.UserRequestDtoToUser(userDto));

    public async Task<bool> Edit(string login, UserRequestDto userDto) =>
        await userDao.Update(login, UserMapper.UserRequestDtoToUser(userDto));

    public async Task<IEnumerable<OrderResponseDto>> GetAllOrders(string login)
    {
        var orders = await orderDao.GetAllByUserLogin(login);
        
        return orders.Select(OrderMapper.OrderToOrderResponseDto);
    }

    public async Task<string?> GetDefaultAddress(string login)
    {
        var user = await userDao.GetByLogin(login);

        return user?.DefaultAddress;
    }

    public async Task<UserResponseDto?> GetUser(string login)
    {
        var user = await userDao.GetByLogin(login);

        return user == null ? null : UserMapper.UserToUserResponseDto(user);
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
