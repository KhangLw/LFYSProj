using Microsoft.AspNetCore.Mvc;
using LFYS_Project.Models;

namespace LFYS_Project.Controllers
{
    public class ContestController : Controller
    {
        WlfysProjContext _context = new WlfysProjContext();
        public ContestController() { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
