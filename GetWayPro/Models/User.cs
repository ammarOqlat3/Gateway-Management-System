using System;
using System.Collections.Generic;

namespace GetWayPro.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? Education { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
