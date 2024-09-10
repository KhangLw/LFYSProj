using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class CategoryExercise
{
    public int CeId { get; set; }

    public int? CategoryId { get; set; }

    public int? ExerciseId { get; set; }

    public virtual CategoryOfExercise? Category { get; set; }

    public virtual Exercise? Exercise { get; set; }
}
