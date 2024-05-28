using Domain.Interfaces;

namespace Domain.Entities;

public partial class Driver : IEntity
{
    public string Guid { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool CertificatAdr { get; set; }

    public virtual Truck? Truck { get; set; }
}