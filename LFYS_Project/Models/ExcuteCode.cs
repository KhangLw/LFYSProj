using LFYS_Project.Models;
using System.Diagnostics;
namespace LFYS_Project.Models
{
    public class ExcuteCode
    {
        public string ExecuteCode(string code, string inputData, string expectedOutput)
        {
            string fileName = "TempProgram.java";
            string className = "TempProgram";
            string output = "";

            try
            {
                // Write the code to a temporary file
                File.WriteAllText(fileName, code);

                // Compile the code
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

                // Execute the compiled code
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
                    writer.WriteLine(inputData);
                }

                output = executor.StandardOutput.ReadToEnd();
                output += executor.StandardError.ReadToEnd();

                executor.WaitForExit();

                // Check if the output matches the expected output
                var isCorrect = output.Trim() == expectedOutput.Trim();
            }
            catch (Exception ex)
            {
                output = "Error: " + ex.Message;
            }
            finally
            {
                // Clean up temporary files
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
        public double IsTrue(CodeSubmission codeSubmission, List<string> intputList, List<string> outputList)
        {

            return 0;
        }
    }
}
