using System;
using System.Collections.Generic;

namespace Trips.Models;

public partial class ContactU
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? MMessage { get; set; }
}
