using System;
using System.Collections.Generic;

namespace Dal;

public partial class User
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string? DefaultAddress { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
