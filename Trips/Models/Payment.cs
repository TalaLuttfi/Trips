using System;
using System.Collections.Generic;

namespace Trips.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TripId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? DatePay { get; set; }

    public virtual Trip? Trip { get; set; }

    public virtual User? User { get; set; }
}
