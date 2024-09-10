using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class CategoryOfExercise
{
    public int CoeId { get; set; }

    public string? CoeName { get; set; }

    public virtual ICollection<CategoryExercise> CategoryExercises { get; set; } = new List<CategoryExercise>();
}
