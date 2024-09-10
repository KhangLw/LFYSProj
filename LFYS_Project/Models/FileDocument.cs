using System;
using System.Collections.Generic;

namespace LFYS_Project.Models;

public partial class FileDocument
{
    public int FiledocId { get; set; }

    public string? FileTitle { get; set; }

    public string? FileContent { get; set; }

    public int? DocumentId { get; set; }

    public virtual Document? Document { get; set; }
}
