using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class UserBadge
{
    public int UserbadgeId { get; set; }

    public string? UserId { get; set; }

    public int? BadgeId { get; set; }

    public DateTime? AwardedAt { get; set; }

    public virtual Badge? Badge { get; set; }
}
