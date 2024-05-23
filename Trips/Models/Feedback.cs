using System;
using System.Collections.Generic;

namespace Trips.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public string? Massage { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
