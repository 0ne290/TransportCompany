namespace Dal.Daos;

public class BaseDao(TransportCompanyContext dbContext) : IDisposable, IAsyncDisposable
{
    public void Dispose()
    {
        dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
    }
}