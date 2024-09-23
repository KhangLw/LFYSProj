﻿using Microsoft.AspNetCore.Mvc;
using LFYS_Project.Models;
using System.Diagnostics.SymbolStore;
using Microsoft.AspNetCore.Identity;
using LFYS_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LFYS_Project.Controllers
{
    public class DocumentController : Controller
    {
        WlfysProjContext _context = new WlfysProjContext();
        private readonly UserManager<AppUser> userManager;
        public DocumentController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
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
                document.UserId = userManager.GetUserId(User);
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
                document.UserId = userManager.GetUserId(User);
                _context.Documents.Update(document);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }


        [Authorize(Roles = "Creater, Admin")]
        public IActionResult AddFile()
        {
            return View();
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
    }
}
