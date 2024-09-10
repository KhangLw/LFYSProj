using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class Video
{
    public int VideoId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? VideoUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }
}
