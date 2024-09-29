using LFYS_Project.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace LFYS_Project.Models
{
    public class ExecuteCode
    {
        public string Execute(string code, List<string> inputDataList, List<string> expectedOutputList)
        {
            string fileName = "TempProgram.java";
            string className = "TempProgram";
            string output = "";
            string pattern = @"class\s+([a-zA-Z_][a-zA-Z0-9_]*)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(code);
            if (match.Success)
            {
                fileName = match.Groups[1].Value + ".java";
                className = match.Groups[1].Value;
            }
            else
            {
                Console.WriteLine("Không tìm thấy class nào trong đoạn code Java.");
                return ("Error");
            }

            try
            {
                File.WriteAllText(fileName, code);
                Process compiler = new Process();
                compiler.StartInfo.FileName = "javac";
                compiler.StartInfo.Arguments = $"-source 1.8 -target 1.8 {fileName}";
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.StartInfo.RedirectStandardError = true;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.CreateNoWindow = true;
                compiler.Start();
                compiler.WaitForExit();

                if (compiler.ExitCode != 0)
                {
                    return "Compilation Failed:\n" + compiler.StandardError.ReadToEnd();
                }
                for (int i = 0; i < inputDataList.Count; i++)
                {
                    Process executor = new Process();
                    executor.StartInfo.FileName = "java";
                    executor.StartInfo.Arguments = className;
                    executor.StartInfo.RedirectStandardOutput = true;
                    executor.StartInfo.RedirectStandardError = true;
                    executor.StartInfo.RedirectStandardInput = true;
                    executor.StartInfo.UseShellExecute = false;
                    executor.StartInfo.CreateNoWindow = true;
                    executor.Start();

                    using (StreamWriter writer = executor.StandardInput)
                    {
                        writer.WriteLine(inputDataList[i]); // Truyền input hiện tại
                    }
                    string programOutput = executor.StandardOutput.ReadToEnd();
                    programOutput += executor.StandardError.ReadToEnd();
                    executor.WaitForExit();

                    // So sánh output thực tế với output mong đợi
                    if (programOutput.Trim() != expectedOutputList[i].Trim())
                    {
                        output += $"Test {i + 1}: Failed\nExpected: {expectedOutputList[i].Trim()}\nGot: {programOutput.Trim()}\n\n";
                    }
                    else
                    {
                        output += $"Test {i + 1}: Passed\n";
                    }
                }
            }
            catch (Exception ex)
            {
                output = "Error: " + ex.Message;
            }
            finally
            {
                // Xóa file tạm
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                if (File.Exists(className + ".class"))
                {
                    File.Delete(className + ".class");
                }
            }

            return output;
        }

        public double IsTrue(CodeSubmission codeSubmission, List<string> inputList, List<string> outputList, string languege)
        {
            string result = "";
            if(languege == "java")
            {
                result = Execute(codeSubmission.Code, inputList, outputList);
            }
            int passedTests = result.Split("Passed").Length - 1;
            int totalTests = inputList.Count;

            return (double) (passedTests / totalTests) * 100 ;
        }
    }
}
