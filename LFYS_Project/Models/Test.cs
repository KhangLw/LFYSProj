namespace LFYS_Project.Models;
public partial class Test
{
    public int TestId { get; set; }
    public int? ExerciseId { get; set; }
    public string? Intput { get; set; }
    public string? Output { get; set; }
    public virtual Exercise? Exercise { get; set; }
}

