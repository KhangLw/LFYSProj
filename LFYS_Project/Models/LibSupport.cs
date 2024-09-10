namespace LFYS_Project.Models
{
    public class LibSupport
    {
        public string ShortenString(string input = "")
        {
            if (input.Length <= 50)
            {
                return input;
            }
            return input.Substring(0, 50) + "...";
        }
    }
}
