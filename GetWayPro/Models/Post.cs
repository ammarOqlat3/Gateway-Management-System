using System;
using System.Collections.Generic;

namespace GetWayPro.Models;

public partial class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImgPath { get; set; }

    public virtual User User { get; set; } = null!;
}
