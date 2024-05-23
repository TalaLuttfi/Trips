using System;
using System.Collections.Generic;

namespace Trips.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
