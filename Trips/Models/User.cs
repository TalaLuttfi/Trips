using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trips.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Fullname { get; set; }

    public string? Username { get; set; }

    public string? UPassword { get; set; }

    public string? PhoneNum { get; set; }

    public string? Gender { get; set; }

    public string? Imagepath { get; set; }
    [ NotMapped]
	public IFormFile? ImageFile { get; set; }
	public int? RoleId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Role? Role { get; set; }
}
