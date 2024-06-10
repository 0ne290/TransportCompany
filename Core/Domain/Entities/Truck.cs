namespace Domain.Entities;

// В идеале, для определения списка допустимыж к перевозке классов опасности, надо добавить кучу bool-полей - если true, то фура может перевозить грузы с таким классом опасности. Также надо разделить сущность "Фура" на несколько сущностей по признаку TypeAdr - EXII, EXIII, обычная фура и т. д.
public class Truck
{
    public override string ToString() => Number;

    public decimal CalculateOrderPrice(Order order)
    {
        var weightPricePerKilometer = WeightPrice * order.CargoWeight;
        var volumePricePerKilometer = VolumePrice * order.CargoVolume;
        var totalPricePerKilometer = (weightPricePerKilometer + volumePricePerKilometer) * PricePerKilometer;
        var orderPrice = totalPricePerKilometer * order.LengthInKilometers;

        return orderPrice;
    }

    public string Number { get; set; } = null!;

    public string TypeAdr { get; set; } = null!;
    
    public bool IsAvailable { get; set; }

    public decimal VolumeMax { get; set; }
    
    public decimal VolumePrice { get; set; }

    public decimal WeightMax { get; set; }
    
    public decimal WeightPrice { get; set; }
    
    public decimal PricePerKilometer { get; set; }

    public string? DriverGuid { get; set; }

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}