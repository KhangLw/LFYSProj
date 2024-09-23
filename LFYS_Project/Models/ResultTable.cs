﻿using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class ResultTable
{
    public int ResultId { get; set; }

    public int? ExerciseId { get; set; }

    public double? Complete { get; set; }

    public string? UserId { get; set; }

    public string? Language { get; set; }

    public DateTime? SubmitTime { get; set; }

    public string? Code { get; set; }

    public virtual Exercise? Exercise { get; set; }
}
