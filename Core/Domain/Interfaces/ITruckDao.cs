using Domain.Entities;

namespace Domain.Interfaces;

public interface ITruckDao : IDisposable, IAsyncDisposable
{
    Task<List<Truck>> GetRightTrucksForOrder(Order order);

    Task Update(Truck truck);
}