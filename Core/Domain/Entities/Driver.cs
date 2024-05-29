namespace Domain.Entities;

public class Driver
{
    public Driver()
    {
        Guid = System.Guid.NewGuid().ToString();
    }

    private Driver(string guid)
    {
        Guid = guid;
    }
    
    public string Guid { get; } = null!;

    public string Name { get; set; } = null!;

    public bool CertificatAdr { get; set; }

    public virtual Truck? Truck { get; set; }
}