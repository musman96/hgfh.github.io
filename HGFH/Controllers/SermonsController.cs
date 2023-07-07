using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HGFH.Data;
using HGFH.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace HGFH.Controllers
{
    [Authorize(Roles ="Admin,Media")]
    public class SermonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SermonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IndexViewModel headerInfo()
        {
            var yearlyDeclaration = _context.Blogs.Where(x => x.CategoryId == 2).OrderByDescending(x => x.CreatedOn).Take(1);
            var monthlyDeclaration = _context.Blogs.Where(x => x.CategoryId == 3).OrderByDescending(x => x.CreatedOn).Take(1);
            var yearBlog = new BlogViewModel();
            var monthlyBlog = new BlogViewModel();
            foreach (var item in yearlyDeclaration)
            {
                yearBlog = new BlogViewModel()
                {
                    Author = item.Author,
                    AuthorId = item.AuthorId,
                    Body = item.Body,
                    Id = item.Id,
                    Summary = item.Summary,
                    Title = item.Title,

                };
            }

            foreach (var item in monthlyDeclaration)
            {
                monthlyBlog = new BlogViewModel()
                {
                    Author = item.Author,
                    AuthorId = item.AuthorId,
                    Body = item.Body,
                    Id = item.Id,
                    Summary = item.Summary,
                    Title = item.Title,

                };
            }

            var index = new IndexViewModel()
            {
                YearlyDeclarationBlog = yearBlog,
                MonthylDeclarationBlog = monthlyBlog

            };

            return index;
        }
        // GET: Sermons
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sermons.Include(s => s.Thumbnail).OrderByDescending(x => x.CreatedOn);
            var index = headerInfo();
            index.Sermons = await applicationDbContext.ToListAsync();
            return View("Index", index);
        }

        // GET: Sermons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sermon = await _context.Sermons
                .Include(s => s.Thumbnail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sermon == null)
            {
                return NotFound();
            }

            return View(sermon);
        }

        // GET: Sermons/Create
        public IActionResult Create()
        {
            ViewData["ThumbnailId"] = new SelectList(_context.Images, "Id", "Id");
            return View();
        }

        // POST: Sermons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,YoutubeLink,Subtitle,Preacher")] Sermon sermon, IFormFile thumbnail)
        {
            if (ModelState.IsValid)
            {
                IFormFile uploadedThumbnailImage;

                MemoryStream ms = new MemoryStream();
                Guid ImageID = new Guid();
                ms = new MemoryStream();
                uploadedThumbnailImage = thumbnail;
                uploadedThumbnailImage.OpenReadStream().CopyTo(ms);

                var imageEntity = new Image()
                {
                    Id = Guid.NewGuid(),
                    Thumbnail = ms.ToArray(),
                    ThumbnailContentType = uploadedThumbnailImage.ContentType,
                    CreatedBy = "System",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "System",
                    ModifiedOn = DateTime.Now
                };
                if (uploadedThumbnailImage.ContentType == "image/jpeg" || uploadedThumbnailImage.ContentType == "image/png" || uploadedThumbnailImage.ContentType == "image/jpg" || uploadedThumbnailImage.ContentType == "image/tif")
                {
                    _context.Images
                        .Add(imageEntity);
                    _context.SaveChanges();
                }

                ImageID = imageEntity.Id;
                sermon.Id = Guid.NewGuid();
                sermon.ModifiedBy = User.Identity.Name;
                sermon.ModifiedOn = DateTime.Now;
                sermon.CreatedBy = User.Identity.Name;
                sermon.CreatedOn = DateTime.Now;
                sermon.ThumbnailId = ImageID;
                _context.Add(sermon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(sermon);
        }

        // GET: Sermons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sermon = await _context.Sermons.FindAsync(id);
            if (sermon == null)
            {
                return NotFound();
            }
            ViewData["ThumbnailId"] = new SelectList(_context.Images, "Id", "Id", sermon.ThumbnailId);
            return View(sermon);
        }

        // POST: Sermons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,YoutubeLink,Subtitle,Body,ThumbnailId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Sermon sermon)
        {
            if (id != sermon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sermon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SermonExists(sermon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ThumbnailId"] = new SelectList(_context.Images, "Id", "Id", sermon.ThumbnailId);
            return View(sermon);
        }

        // GET: Sermons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sermon = await _context.Sermons
                .Include(s => s.Thumbnail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sermon == null)
            {
                return NotFound();
            }

            return View(sermon);
        }

        // POST: Sermons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sermon = await _context.Sermons.FindAsync(id);
            _context.Sermons.Remove(sermon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SermonExists(Guid id)
        {
            return _context.Sermons.Any(e => e.Id == id);
        }

        [HttpGet]
        [AllowAnonymous]
        public FileStreamResult ThumbnailImage(Guid id)
        {
            using (_context)
            {
                Image image = _context.Images.FirstOrDefault(m => m.Id == id);

                MemoryStream ms = new MemoryStream(image.Thumbnail);

                return new FileStreamResult(ms, image.ThumbnailContentType);
            }
        }
    }
}
