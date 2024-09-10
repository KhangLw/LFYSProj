using LFYS_Project.Models;
using LFYS_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace LFYS_Project.Controllers
{
    public class ExecutorCodeController : Controller
    {
        private readonly CodeExecutionService _codeExecutionService;

        public ExecutorCodeController()
        {
            _codeExecutionService = new CodeExecutionService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new CodeSubmission());
        }

        [HttpPost]
        public IActionResult Index(string code)
        {
            CodeSubmission codeSubmission = new CodeSubmission();
            codeSubmission.Code = code;
            //codeSubmission.ExpectedOutput = "4";
            //codeSubmission.ActualOutput = _codeExecutionService.ExecuteCode(codeSubmission.Code, codeSubmission.InputData, out bool isCorrect, codeSubmission.ExpectedOutput);
            //codeSubmission.IsCorrect = isCorrect;

            return View();
        }
    }
}
