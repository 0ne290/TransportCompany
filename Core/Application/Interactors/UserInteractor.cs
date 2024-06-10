using Application.Dtos;
using Application.Mappers;
using Domain.Interfaces;

namespace Application.Interactors;

public class UserInteractor(IUserDao userDao, IOrderDao orderDao, ITruckDao truckDao) : IDisposable, IAsyncDisposable
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
    
    public async Task<bool> CreateOrder(OrderRequestDto orderDto)
    {
        var order = OrderMapper.OrderRequestDtoToOrder(orderDto);

        var rightTrucks = await truckDao.GetRightTrucksForOrder(order);
        if (rightTrucks.Count == 0)
            return false;

        var minPrice = rightTrucks[0].CalculateOrderPrice(order);
        var maxProfitTruck = rightTrucks[0];
        foreach (var currentTruck in rightTrucks)
        {
            var currentPrice = currentTruck.CalculateOrderPrice(order);
            
            if (currentPrice >= minPrice)
                continue;
            
            minPrice = currentPrice;
            maxProfitTruck = currentTruck;
        }

        order.Price = minPrice;
        order.TruckNumber = maxProfitTruck.Number;
        maxProfitTruck.IsAvailable = false;

        await truckDao.Update(maxProfitTruck);
        await orderDao.Create(order);

        return true;
    }

    public void Dispose()
    {
        userDao.Dispose();
        orderDao.Dispose();
        truckDao.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await userDao.DisposeAsync();
        await orderDao.DisposeAsync();
        await truckDao.DisposeAsync();
    }
}
