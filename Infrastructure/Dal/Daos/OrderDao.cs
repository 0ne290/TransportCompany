using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dal.Daos;

public class OrderDao(TransportCompanyContext dbContext) : BaseDao(dbContext), IOrderDao
{
    public async Task<IEnumerable<Order>> GetAllByUserLogin(string userLogin) =>
        await dbContext.Orders.AsNoTracking().Where(o => o.UserLogin == userLogin).ToListAsync();
    
    public async Task Create(Order order)
    {
        await dbContext.Orders.AddAsync(order);
        
        await dbContext.SaveChangesAsync();
    }
}