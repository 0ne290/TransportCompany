using Domain.Interfaces;

namespace Dal;

public partial class Order : IEntity
{
    public string Guid { get; set; } = null!;

    public DateTime DateBegin { get; set; }

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
