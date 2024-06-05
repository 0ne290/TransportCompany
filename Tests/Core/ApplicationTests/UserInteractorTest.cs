using Application.Interactors;
using Application.Mappers;
using Dal;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ApplicationTests;

public class UserInteractorTest
{
    public UserInteractorTest()
    {
        PrepareTestData();
    }

    private void PrepareTestData()
    {
        _testOrders = new Order[]
        {
            new()
            {
                DateEnd = new DateTime(2002, 9, 10, 14, 37, 22), Address = "234Something432", Price = 37,
                CargoVolume = 73, CargoWeight = 21
            },
            new()
            {
                DateEnd = new DateTime(2019, 11, 1, 21, 12, 8), Address = "432234", Price = 2, CargoVolume = 2,
                CargoWeight = 2
            },
            new()
            {
                DateEnd = new DateTime(1999, 5, 13, 17, 19, 3), Address = "234432", Price = 81, CargoVolume = 7,
                CargoWeight = 15
            }
        };
        
        _testTrucks = new Truck[]
        {
            new()
            {
                Number = "eubr5", TypeAdr = "ftueb", VolumeMax = 35, WeightMax = 12, DriverGuid = null, Driver = null,
                Orders = new[] { _testOrders[0] }
            },
            new()
            {
                Number = "rgdv", TypeAdr = "24sfg", VolumeMax = 3, WeightMax = 22, DriverGuid = null, Driver = null,
                Orders = new[] { _testOrders[1] }
            },
            new()
            {
                Number = "zqws", TypeAdr = "ppln", VolumeMax = 14, WeightMax = 29, DriverGuid = null, Driver = null,
                Orders = new[] { _testOrders[2] }
            }
        };
        
        _testUsers = new User[]
        {
            new()
            {
                Login = "igru", Password = "234df", Name = "24gkojg", Contact = "122efhb", DefaultAddress = null,
                Orders = new[] { _testOrders[0] }
            },
            new()
            {
                Login = "t7hfg", Password = "114edbb", Name = "p89orsfb", Contact = "gvxe145fh", DefaultAddress = null,
                Orders = new[] { _testOrders[1] }
            },
            new()
            {
                Login = "gscvzt", Password = "y337hdgljc", Name = "236idhnsa3", Contact = "nxdubsr", DefaultAddress = null,
                Orders = new[] { _testOrders[2] }
            }
        };
        
        _testOrders[0].UserLogin = _testUsers[0].Login;
        _testOrders[0].TruckNumber = _testTrucks[0].Number;
        _testOrders[0].TruckNumberNavigation = _testTrucks[0];
        _testOrders[0].UserLoginNavigation = _testUsers[0];
        _testOrders[1].UserLogin = _testUsers[1].Login;
        _testOrders[1].TruckNumber = _testTrucks[1].Number;
        _testOrders[1].TruckNumberNavigation = _testTrucks[1];
        _testOrders[1].UserLoginNavigation = _testUsers[1];
        _testOrders[2].UserLogin = _testUsers[2].Login;
        _testOrders[2].TruckNumber = _testTrucks[2].Number;
        _testOrders[2].TruckNumberNavigation = _testTrucks[2];
        _testOrders[2].UserLoginNavigation = _testUsers[2];
    }
    
    [Fact]
    public async Task GetAllOrders_ThreeOrders_ReturnsThreeOrdersResponseDto()
    {
        // Arrange
        var userDaoMock = new Mock<IUserDao>();
        var orderDaoMock = new Mock<IOrderDao>();
        orderDaoMock.Setup(orderDao => orderDao.GetAllByUserLogin(It.IsAny<string>())).ReturnsAsync(_testOrders);
        
        var userInteractor = new UserInteractor(userDaoMock.Object, orderDaoMock.Object);
        var orderMapper = new OrderMapper();
        var expected = _testOrders.Select(o => orderMapper.OrderToOrderResponseDto(o)).ToList();

        // Act
        var actual = (await userInteractor.GetAllOrders(string.Empty)).ToList();

        // Assert
        Assert.True(expected.Count == actual.Count);
        for (var i = 0; i < expected.Count; i++)
            Assert.True(expected[i].Equals(actual[i]));
    }
    
    /*[Fact]
    public async Task Xyz()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TransportCompanyContext>();
        const string connectionString = "Server=localhost;Database=TransportCompany;Uid=root;Pwd=!IgEcA21435=;";
        var options = optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
        await using var context = new TransportCompanyContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        var order = new Order
        {
            DateEnd = new DateTime(2002, 9, 10), Address = "234Something432", Price = 37,
            CargoVolume = 73, CargoWeight = 21
        };
        var truck = new Truck
        {
            Number = "eubr5", TypeAdr = "EXII", VolumeMax = 35, WeightMax = 12, DriverGuid = null, Driver = null,
        };
        var user = new User
        {
            Login = "igru", Password = "234df", Name = "24gkojg", Contact = "122efhb", DefaultAddress = null,
        };
        order.UserLogin = user.Login;
        order.TruckNumber = truck.Number;

        await context.Trucks.AddAsync(truck);
        await context.Users.AddAsync(user);
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }*/

    private Order[] _testOrders = null!;
    
    private Truck[] _testTrucks = null!;
    
    private User[] _testUsers = null!;
}
