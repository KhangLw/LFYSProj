using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LFYS_Project.Data;
using LFYS_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace LFYS_Project.Controllers
{
    public class CreaterController : Controller
    {
        WlfysProjContext _context = new WlfysProjContext();
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CreaterController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Document()
        {
            var user = await _userManager.GetUserAsync(User);
            var documents = _context.Documents.Where(u => u.UserId == user.Id).Include(d => d.Category).ToList();
            return View(documents);
        }

        public async Task<IActionResult> Exercise()
        {
            var user = await _userManager.GetUserAsync(User);
            var exercises = _context.Exercises.Where(u => u.UserId == user.Id).ToList();
            return View(exercises);
        }
        public async Task<IActionResult> Course()
        {
            var user = await _userManager.GetUserAsync(User);
            var courses = _context.Courses.Where(u => u.UserId == user.Id).Include(c => c.Category).ToList();

            return View(courses);
        }
    }
}
