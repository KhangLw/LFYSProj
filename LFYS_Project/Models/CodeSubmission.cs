namespace LFYS_Project.Models
{
    public class CodeSubmission
    {
        public string Code { get; set; }
        public string InputData { get; set; }
        public string ExpectedOutput { get; set; }
        public string ActualOutput { get; set; }
        public bool IsCorrect { get; set; }
    }
}
