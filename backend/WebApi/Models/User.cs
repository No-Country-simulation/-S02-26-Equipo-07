using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual ICollection<Caballo> Caballos { get; set; } = new List<Caballo>();

    public virtual ICollection<Jinete> Jinetes { get; set; } = new List<Jinete>();

    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}
