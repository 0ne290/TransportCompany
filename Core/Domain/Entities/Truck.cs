using Domain.Interfaces;

namespace Domain.Entities;

public partial class Truck : IEntity
{
    public string Number { get; set; } = null!;

    public string TypeAdr { get; set; } = null!;

    public decimal VolumeMax { get; set; }

    public decimal WeightMax { get; set; }

    public string? DriverGuid { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
