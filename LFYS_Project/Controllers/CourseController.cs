using Microsoft.AspNetCore.Mvc;
using LFYS_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace LFYS_Project.Controllers
{
    //[Route("Course")]
    //[Authorize]
    public class CourseController : Controller
    {
        private readonly WlfysProjContext _context = new WlfysProjContext();
        private readonly UserManager<AppUser> userManager;
        public CourseController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        //[HttpGet("Index")]
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }

        public IActionResult Detail(int id)
        {
            var course = _context.Courses.Find(id);
            return View(course);
        }
        //[HttpGet("Upload")]
        public IActionResult Upload()
        {
            return View();
        }
        //[HttpPost("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] string courseName, [FromForm]  string description, [FromForm] string courseSelected, string isFree, [FromForm] string price, [FromForm] string discount, IFormFile courseAvt)
        {
            var user = userManager.GetUserAsync(User);
            if (ModelState.IsValid) 
            {
                Course course = new Course();
                course.CourseName = courseName;
                course.Description = description;
                course.CategoryId = Convert.ToInt32(courseSelected);
                course.IsFree = isFree == "1" ? true : false;
                course.Price = Convert.ToDouble(price);
                course.Discount = Convert.ToInt32(discount);
                course.Avt = courseAvt.FileName;
                course.UserId = userManager.GetUserId(User);
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        //[HttpGet("Video")]

        public IActionResult Video(int id = 0)
        {
            var video = _context.Videos.Find(id);
            return View(video);
        }

        //[HttpPost("AddVideo")]
        [HttpPost]
        public async Task<IActionResult> AddVideo([FromForm] IFormFile videoUrl, [FromForm] string title, [FromForm] string description, [FromForm] string courseId)
        {
            if (ModelState.IsValid)
            {
                var video = new Video();
                video.Title = title;
                video.Description = description;
                video.CourseId = Convert.ToInt32(courseId);

            }
            return BadRequest();
        }
    }
}
