using Microsoft.AspNetCore.Mvc;
using LFYS_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis.Operations;

namespace LFYS_Project.Controllers
{
    //[Route("Course")]
    //[Authorize]
    public class CourseController : Controller
    {
        private readonly WlfysProjContext _context = new WlfysProjContext();
        private readonly UserManager<AppUser> userManager;
        private readonly IWebHostEnvironment _env;
        public CourseController(UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            this.userManager = userManager;
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
            var user = userManager.GetUserAsync(User);
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
                course.UserId = userManager.GetUserId(User);
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        public IActionResult Video(int id = 0)
        {
            var video = _context.Videos.Find(id);
            return View(video);
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpGet]
        public IActionResult AddVideo()
        {
            return View();
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadVideo(IFormFile videoUrl, string title, string description, string courseId)
        {
            // Kiểm tra dữ liệu nhập vào
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || videoUrl == null)
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }

            // Đường dẫn lưu video
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "videos");

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tạo tên file duy nhất
            var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(videoUrl.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu file video vào thư mục
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await videoUrl.CopyToAsync(fileStream);
            }

            // Lưu file vào db
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
            // Trả về kết quả JSON
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
            // Kiểm tra dữ liệu nhập vào
            if (string.IsNullOrEmpty(courseId) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || videoUrl == null)
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ thông tin." });
            }

            // Đường dẫn lưu video
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "videos");

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tạo tên file duy nhất
            var uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(videoUrl.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu file video vào thư mục
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await videoUrl.CopyToAsync(fileStream);
            }

            // Lưu file vào db
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
            // Trả về kết quả JSON
            return Json(new
            {
                success = true,
                message = "Upload video thành công!",
                videoPath = $"/uploads/videos/{uniqueFileName}"
            });
        }
    }
}
