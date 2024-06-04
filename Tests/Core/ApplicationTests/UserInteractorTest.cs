using Application.Interactors;

namespace ApplicationTests;

public class UserInteractorTest
{
    [Fact]
    public void GetAllOrders_ThreeOrders_ReturnsThreeOrdersResponseDto()
    {
        var userDaoMock = new Mock<IUserDao>();
        var orderDaoMock = new Mock<IOrderDao>();
        var trucks = new[] {  };
        var users = new[] {  };
        var orders = new[] { new Order { DateEnd = new DateTime(2002, 9, 10, 14, 37, 22) }, Address = "234Something432", Price = 37, CargoVolume = 73, CargoWeight = 21, UserLogin = users[0].Login, TruckNumber = trucks[0].Number, TruckNumberNavigation = trucks[0], UserLoginNavigation = users[0] }, new Order { DateEnd = new DateTime(2019, 11, 1, 21, 12, 8) }, Address = "432234", Price = 2, CargoVolume = 2, CargoWeight = 2, UserLogin = users[1].Login, TruckNumber = trucks[1].Number, TruckNumberNavigation = trucks[1], UserLoginNavigation = users[1] }, new Order { DateEnd = new DateTime(1999, 5, 13, 17, 19, 3) }, Address = "234432", Price = 81, CargoVolume = 7, CargoWeight = 15, UserLogin = users[2].Login, TruckNumber = trucks[2].Number, TruckNumberNavigation = trucks[2], UserLoginNavigation = users[2] } };
        orderDaoMock.Setup(orderDao.GetAllByUserLogin(It.IsAny<string>())).Returns(orders);
        var userInteractor = new UserInteractor(userDaoMock, orderDaoMock);
        var orderMapper = new OrderMapper();
        var expected = orders.Select(o => orderMapper.OrderToOrderResponseDto(o));

        var actual = userInteractor.GetAllOrders(_);

        Assert.True(expected.Length == actual.Length);
        for (var i = 0; i < expected.Length; i++)
            Assert.True(expected[i].Equals(actual[i]));
    }
}
