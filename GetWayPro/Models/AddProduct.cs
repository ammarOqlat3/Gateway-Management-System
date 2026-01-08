using System;
using System.Collections.Generic;

namespace GetWayPro.Models;

public partial class AddProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public double Price { get; set; }

    public string? Description { get; set; }
    public string? ImgPath { get; set; }

    // الربط بالمستخدم
    public int UserId { get; set; }
}
