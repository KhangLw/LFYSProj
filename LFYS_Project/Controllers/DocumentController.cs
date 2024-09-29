using Microsoft.AspNetCore.Mvc;
using LFYS_Project.Models;
using System.Diagnostics.SymbolStore;
using Microsoft.AspNetCore.Identity;
using LFYS_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LFYS_Project.Controllers
{
    public class DocumentController : Controller
    {
        WlfysProjContext _context = new WlfysProjContext();
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DocumentController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var documents = _context.Documents.ToList();
            return View(documents);
        }
        public IActionResult Detail(int id = 0)
        {
            var document = _context.Documents.Find(id);
            return View(document);
        }

        [Authorize(Roles = "Creater, Admin")]
        public IActionResult Upload()
        {
            return View();
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> Upload(string title, string content, string selectedCourse)
        {
            if(ModelState.IsValid)
            {
                Document document = new Document();
                document.Title = title;
                document.Description = content;
                document.CategoryId = Convert.ToInt32(selectedCourse);
                document.UserId = _userManager.GetUserId(User);
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            var document = _context.Documents.Find(id);
            return View(document);
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string title, string content, string selectedCourse)
        {
            if (ModelState.IsValid)
            {
                Document document = new Document();
                document.DocumentId = id;
                document.Title = title;
                document.Description = content;
                document.CategoryId = Convert.ToInt32(selectedCourse);
                document.UserId = _userManager.GetUserId(User);
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }


        [Authorize(Roles = "Creater, Admin")]
        public async Task<IActionResult> AddFile()
        {
            var user = await _userManager.GetUserAsync(User);
            var documents = _context.Documents.Where(u => u.UserId == user.Id).Include(d => d.Category).ToList();
            return View(documents);
        }


        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddFile(string title, string content, string selectedDocument)
        {
            if (ModelState.IsValid)
            {
                FileDocument fileDoc = new FileDocument();
                fileDoc.FileTitle = title;
                fileDoc.FileContent = content;
                fileDoc.DocumentId = Convert.ToInt32(selectedDocument);
                _context.FileDocuments.Add(fileDoc);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Creater, Admin")]
        public IActionResult UpdateFile(int id)
        {
            var file = _context.FileDocuments.Include(fd => fd.Document).FirstOrDefault(f => f.FiledocId == id);
            return View(file);
        }
        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateFile(int id, string title, string content, string selectedDocument)
        {
            var fileDoc = await _context.FileDocuments.FindAsync(id);

            if (fileDoc == null)
            {
                return NotFound(new { message = "File document không tồn tại" });
            }

            if (ModelState.IsValid)
            {
                fileDoc.FileTitle = title;
                fileDoc.FileContent = content;
                fileDoc.DocumentId = Convert.ToInt32(selectedDocument);
                _context.FileDocuments.Update(fileDoc);
                await _context.SaveChangesAsync();
                return Ok(new { message = "File document đã được cập nhật thành công" });
            }

            return BadRequest(new { message = "Dữ liệu không hợp lệ" });
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Tìm course dựa trên ID
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            var fileDoc = _context.FileDocuments.Where(v => v.DocumentId == id).ToList();
            _context.FileDocuments.RemoveRange(fileDoc);
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            // Trả về kết quả sau khi xóa thành công
            return Ok(new { message = "Tài liệu đã được xóa thành công" });
        }

        [Authorize(Roles = "Creater, Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = await _context.FileDocuments.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            _context.FileDocuments.Remove(file);
            await _context.SaveChangesAsync();

            // Trả về kết quả sau khi xóa thành công
            return Ok(new { message = "Bài học đã được xóa thành công" });
        }
    }
}
