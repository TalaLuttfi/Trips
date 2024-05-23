using System;
using System.Collections.Generic;

namespace Trips.Models;

public partial class VisaCard
{
    public int Id { get; set; }

    public string? HolderName { get; set; }

    public string? CardNum { get; set; }

    public DateTime? ExpDate { get; set; }

    public string? Cvv { get; set; }

    public decimal? Balance { get; set; }
}
