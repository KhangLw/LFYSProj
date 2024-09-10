using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
