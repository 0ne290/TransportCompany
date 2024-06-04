using Application.Interactors;

namespace ApplicationTests;

public class UserInteractorTest
{
    [Fact]
    public void GetAllOrders_ExistingUser_UserOrders()
    {
        var userInteractor = new UserInteractor();// С помощью Moq передать правильно "зафейкованные" зависимости
    }
}