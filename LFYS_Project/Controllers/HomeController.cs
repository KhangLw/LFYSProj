using LFYS_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LFYS_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["IsCreater"] = false;
            var user = await _userManager.GetUserAsync(User);
            var createrUsers = await _userManager.GetUsersInRoleAsync("Creater");
            createrUsers = createrUsers.Take(4).ToList();
            if(user != null) 
            {
                var isCreater = _userManager.IsInRoleAsync(user, "Creater");
                ViewData["IsCreater"] = isCreater;
            }
            
            return View(createrUsers);
        }
        public async Task<IActionResult> About()
        {
            ViewData["IsCreater"] = false;
            var user = await _userManager.GetUserAsync(User);
            var createrUsers = await _userManager.GetUsersInRoleAsync("Creater");
            createrUsers = createrUsers.Take(4).ToList();
            if (user != null)
            {
                var isCreater = _userManager.IsInRoleAsync(user, "Creater");
                ViewData["IsCreater"] = isCreater;
            }

            return View(createrUsers);
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
