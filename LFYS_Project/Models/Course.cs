using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public bool? IsFree { get; set; }

    public double? Price { get; set; }

    public int? Discount { get; set; }

    public string? Avt { get; set; }

    public string? UserId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
