﻿using Domain.Constants;

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

    public DateTime? DateEnd
    {
        get => _dateEnd;
        set
        {
            _dateEnd = value;
            if (TruckNumberNavigation != null)
                TruckNumberNavigation.IsAvailable = true;
        }
    }

    public string Address { get; set; } = null!;
    
    public decimal LengthInKilometers { get; set; }

    public decimal ClassAdr
    {
        get => _classAdr;
        set
        {
            if (!ClassesAdr.ClassExists(value))
                throw new ArgumentException($"{value} is not an class adr", nameof(value));
            _classAdr = value;
        }
    }

    public decimal Price { get; set; }

    public decimal CargoVolume { get; set; }

    public decimal CargoWeight { get; set; }

    // Внешние ключи для навигационных свойств. Они не могут быть null и должны указывать на существующщие записи
    public string UserLogin { get; set; } = null!;

    public string TruckNumber { get; set; } = null!;

    // Навигационные свойства. Как видно, они могут быть null - с этим нелепым парадоксом приходится мириться, т. к. иначе будет невозможно создавать заказы в CoreAdmin. Решение - максимально минимизировать API Fluent и максимизировать Data Annotations
    public virtual Truck? TruckNumberNavigation { get; set; }

    public virtual User? UserLoginNavigation { get; set; }

    private decimal _classAdr;

    private DateTime? _dateEnd;
}
