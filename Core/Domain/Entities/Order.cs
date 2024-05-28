namespace Domain.Entities;

public class Order
{
    public Order()
    {
        Guid = System.Guid.NewGuid().ToString();
        DateBegin = DateTime.Now;
    }

    private Order(string guid, DateTime dateBegin)
    {
        Guid = guid;
        DateBegin = dateBegin;
    }
    
    public string Guid { get; } = null!;

    public DateTime DateBegin { get; }

    public DateTime? DateEnd { get; set; }

    public string Address { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal CargoVolume { get; set; }

    public decimal CargoWeight { get; set; }

    public string UserLogin { get; set; } = null!;

    public string TruckNumber { get; set; } = null!;

    public virtual Truck TruckNumberNavigation { get; set; } = null!;

    public virtual User UserLoginNavigation { get; set; } = null!;
}
