using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trips.Models;

public partial class Trip
{
    public int Id { get; set; }

    public string? TripName { get; set; }

    public string? Destination { get; set; }

    public int? Cost { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? StartDestination { get; set; }

    public string? FinalDestination { get; set; }

    public string? ImagePath { get; set; }

    public string? TDescription { get; set; }
    public int? CategoryID { get; set; }
    [NotMapped]
    public string? CategoryName { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual Category? Category { get; set; }
}
