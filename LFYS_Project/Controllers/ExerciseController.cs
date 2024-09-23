using LFYS_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using System.CodeDom;

namespace LFYS_Project.Controllers
{
    [Authorize]
    public class ExerciseController : Controller
    {
        WlfysProjContext _context = new WlfysProjContext();
        private readonly UserManager<AppUser> userManager;
        public ExerciseController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var exercises = _context.Exercises.ToList();
            return View(exercises);
        }
        public IActionResult Detail(int id = 0)
        {
            if (id == 0) return BadRequest();
            var exercise = _context.Exercises.Find(id);
            return View(exercise);
        }

        [Authorize(Roles = "Creater, Admin")]
        public IActionResult Upload()
        {
            return View();
        }

        [Authorize]
        public IActionResult Upcode()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Creater")]
        public IActionResult Delete()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Creater")]
        public IActionResult Update(int id)
        {
            var exercise = _context.Exercises.Include(e => e.Tests).FirstOrDefault(e => e.ExerciseId == id);
            return View(exercise);
        }

        [Authorize(Roles = "Admin, Creater")]
        [HttpPost]
        public async Task<IActionResult> SaveUpdate([FromBody] DocumentViewModelUpdate model)
        {
            if (ModelState.IsValid)
            {
                Exercise ex = new Exercise();
                ex.ExerciseId = model.Id;
                ex.Title = model.Title;
                ex.Description = model.Content;
                ex.CreatedAt = DateTime.Now;
                ex.UpdatedAt = DateTime.Now;
                ex.UserId = userManager.GetUserId(User);

                _context.Exercises.Update(ex);
                await _context.SaveChangesAsync();

                var catExs = _context.CategoryExercises.Where(c => c.ExerciseId == model.Id).ToList();
                _context.CategoryExercises.RemoveRange(catExs);

                var tests = _context.Tests.Where(t => t.ExerciseId == model.Id).ToList();
                _context.Tests.RemoveRange(tests);

                foreach (var category in model.Categories)
                {
                    CategoryExercise catOfEx = new CategoryExercise();
                    catOfEx.CategoryId = category;
                    catOfEx.ExerciseId = ex.ExerciseId;
                    _context.CategoryExercises.Add(catOfEx);
                }
                foreach (var test in model.TestCases)
                {
                    Test tst = new Test();
                    tst.ExerciseId = ex.ExerciseId;
                    tst.Intput = test.Input;
                    tst.Output = test.Output;
                    _context.Tests.Add(tst);
                }
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            return BadRequest(new { success = false, message = "Invalid data" });
        }

        public IActionResult ResultTable()
        {
            var results = _context.ResultTables.Include(r => r.Exercise).ToList();
            return View(results);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEx([FromBody] DocumentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Exercise ex = new Exercise();
                ex.Title = model.Title;
                ex.Description = model.Content;
                ex.CreatedAt = DateTime.Now;
                ex.UpdatedAt = DateTime.Now;
                ex.UserId = userManager.GetUserId(User);

                _context.Exercises.Add(ex);
                await _context.SaveChangesAsync();

                foreach (var category in model.Categories)
                {
                    CategoryExercise catOfEx = new CategoryExercise();
                    catOfEx.CategoryId = category;
                    catOfEx.ExerciseId = ex.ExerciseId;
                    _context.CategoryExercises.Add(catOfEx);
                }
                foreach(var test in model.TestCases)
                {
                    Test tst = new Test();
                    tst.ExerciseId = ex.ExerciseId;
                    tst.Intput = test.Input;
                    tst.Output = test.Output;
                    _context.Tests.Add(tst);
                }
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            return BadRequest(new { success = false, message = "Invalid data" });
        }
    }
    public class DocumentViewModelUpdate
    {
        public int Id { get; set; }
        public List<int> Categories { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<TestCase> TestCases { get; set; }
    }
    public class DocumentViewModel
    {
        public List<int> Categories { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<TestCase> TestCases { get; set; }
    }
    public class TestCase
    {
        public string Input { get; set; }
        public string Output { get; set; }
    }
}
