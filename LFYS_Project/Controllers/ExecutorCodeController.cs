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
            return View(id);
        }

        [HttpPost]
        public IActionResult Execute(string code, string language, int id)
        {
            ExecuteCode excuteCode = new ExecuteCode();
            CodeSubmission codeSubmission = new CodeSubmission();
            codeSubmission.Code = code;
            var inputList = _context.Tests.Where(c => c.ExerciseId == id).Select(c => c.Intput).ToList();
            var outputList = _context.Tests.Where(c => c.ExerciseId == id).Select(c => c.Output).ToList() ;
            var output = excuteCode.IsTrue(codeSubmission, inputList, outputList, language);

            ResultTable result = new ResultTable();
            result.ExerciseId = id;
            result.Language = language;
            result.Complete = output;
            result.SubmitTime = DateTime.Now;

            try
            {
                _context.ResultTables.Add(result);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return Json(new { redirectUrl = Url.Action("ResultTable", "Exercise") });
        }

    }
}
