using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class Document
{
    public int DocumentId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UserId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<FileDocument> FileDocuments { get; set; } = new List<FileDocument>();
}
