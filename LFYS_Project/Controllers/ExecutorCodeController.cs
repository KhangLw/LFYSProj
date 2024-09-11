using LFYS_Project.Models;
using LFYS_Project.Services;
using Microsoft.AspNetCore.Mvc;
using LFYS_Project.Models;

namespace LFYS_Project.Controllers
{
    public class ExecutorCodeController : Controller
    {
        private readonly CodeExecutionService _codeExecutionService;
        private readonly WlfysProjContext _context = new WlfysProjContext();
        public ExecutorCodeController()
        {
            _codeExecutionService = new CodeExecutionService();
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            return View(Convert.ToDouble(id + 100));
        }

        [HttpPost]
        public IActionResult Index(string code, string language, int id)
        {
            ExecuteCode excuteCode = new ExecuteCode();
            CodeSubmission codeSubmission = new CodeSubmission();
            codeSubmission.Code = code;
            var inputList = _context.Tests.Where(c => c.ExerciseId == id).Select(c => c.Intput).ToList();
            var outputList = _context.Tests.Where(c => c.ExerciseId == id).Select(c => c.Output).ToList() ;
            var output = excuteCode.IsTrue(codeSubmission, inputList, outputList);

            //codeSubmission.ExpectedOutput = "4";
            //codeSubmission.ActualOutput = _codeExecutionService.ExecuteCode(codeSubmission.Code, codeSubmission.InputData, out bool isCorrect, codeSubmission.ExpectedOutput);
            //codeSubmission.IsCorrect = isCorrect;

            return View(output);
        }

    }
}
