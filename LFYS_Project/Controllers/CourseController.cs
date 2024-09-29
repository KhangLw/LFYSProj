using Microsoft.AspNetCore.Mvc;
using LFYS_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace LFYS_Project.Controllers
{
    //[Route("Course")]
    //[Authorize]
    public class CourseController : Controller
    {
        private readonly WlfysProjContext _context = new WlfysProjContext();
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IWebHostEnvironment _env;
        public CourseController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
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

        [Authorize(Roles = "Creater, Admin")]
        public IActionResult Upload()
        {
            return View();
        }
        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] string courseName, [FromForm] string description, [FromForm] string courseSelected, string isFree, [FromForm] string price, [FromForm] string discount, IFormFile courseAvt)
        {
            var user = _userManager.GetUserAsync(User);
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
                course.UserId = _userManager.GetUserId(User);
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var course = _context.Courses.Find(id);
            return View(course);
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] string courseId, [FromForm] string courseName, [FromForm] string description, [FromForm] string courseSelected, string isFree, [FromForm] string price, [FromForm] string discount, IFormFile courseAvt)
        {
            var user = _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                Course course = new Course();
                course.CourseId = Convert.ToInt32(courseId);
                course.CourseName = courseName;
                course.Description = description;
                course.CategoryId = Convert.ToInt32(courseSelected);
                course.IsFree = isFree == "1" ? true : false;
                course.Price = Convert.ToDouble(price);
                course.Discount = Convert.ToInt32(discount);
                course.Avt = courseAvt.FileName;
                course.UserId = _userManager.GetUserId(User);
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Tìm course dựa trên ID
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var video = _context.Videos.Where(v => v.CourseId == id).ToList();
            _context.Videos.RemoveRange(video);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            // Trả về kết quả sau khi xóa thành công
            return Ok(new { message = "Khóa học đã được xóa thành công" });
        }
        public IActionResult Video(int id = 0)
        {
            var video = _context.Videos.Find(id);
            return View(video);
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpGet]
        public async Task<IActionResult> AddVideo()
        {
            var user = await _userManager.GetUserAsync(User);
            var courses = _context.Courses.Where(u => u.UserId == user.Id).Include(c => c.Category).ToList();
            return View(courses);
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadVideo(IFormFile videoUrl, string title, string description, string courseId)
        {
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || videoUrl == null)
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "videos");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(videoUrl.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await videoUrl.CopyToAsync(fileStream);
            }
            var video = new Video
            {
                CourseId = Convert.ToInt32(courseId),
                Title = title,
                Description = description,
                VideoUrl = uniqueFileName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = "Upload video thành công!",
                videoPath = $"/uploads/videos/{uniqueFileName}"
            });
        }

        [Authorize(Roles = "Creater, Admin")]
        public IActionResult UpdateVideo(int id)
        {
            var video = _context.Videos.Find(id);
            return View(video);
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateVideo(IFormFile videoUrl, string title, string description, string courseId, string videoId)
        {
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || videoUrl == null)
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "videos");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(videoUrl.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await videoUrl.CopyToAsync(fileStream);
            }
            var video = new Video
            {
                VideoId = Convert.ToInt32(videoId),
                CourseId = Convert.ToInt32(courseId),
                Title = title,
                Description = description,
                VideoUrl = uniqueFileName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            _context.Videos.Update(video);
            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = "Upload video thành công!",
                videoPath = $"/uploads/videos/{uniqueFileName}"
            });
        }
        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            // Trả về kết quả sau khi xóa thành công
            return Ok(new { message = "Video đã được xóa thành công" });
        }
    }
}
