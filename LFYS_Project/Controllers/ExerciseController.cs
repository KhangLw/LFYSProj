using LFYS_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace LFYS_Project.Controllers
{
    public class ExerciseController : Controller
    {
        WlfysProjContext _context = new WlfysProjContext();
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

        public IActionResult Upload()
        {
            return View();
        }
    }
}
