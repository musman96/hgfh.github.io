using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HGFH.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using HGFH.Models;
using HGFH.Services;

namespace HGFH.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BlogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IEmailSenderService emailSenderService ;
        public BlogsController(ApplicationDbContext context, IEmailSenderService emailSender)
        {
            _context = context;
            emailSenderService = emailSender;
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

        // GET: Blogs
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Blogs.Include(b => b.Author).OrderByDescending(x=> x.CreatedOn);
            var index = headerInfo();
            index.Blogs = await applicationDbContext.ToListAsync();
            return View("Index",index);
        }

        // GET: Blogs/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var categories = _context.Options.Where(x => x.OptionGroupId == 1).ToList(); // get the list of categories filtered out by the option group id
            //get blog details
            var blog = await _context.Blogs
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            var recentBlogs = _context.Blogs
                .Include(b => b.Author).OrderByDescending(x => x.CreatedOn).Take(3).ToList();
            var allBlogs = _context.Blogs
                .Include(b => b.Author).ToList();

            var comments = _context.Comments.Where(x => x.BlogPostId == id).ToList();

            var subcomments = _context.SubComments.Where(x => x.BlogPostId == id).ToList();

            var commentCount = comments.Count + subcomments.Count;

            if (blog == null)
            {
                return NotFound();
            }


            // map the blog object to the blog view model
            var blogModel = new BlogViewModel()
            {
                Author = blog.Author,
                AuthorId = blog.AuthorId,
                Body = blog.Body,
                Categories = categories,
                CateoryId = blog.CategoryId,
                CreatedBy = blog.CreatedBy,
                CreatedOn = blog.CreatedOn,
                ModifiedBy = blog.ModifiedBy,
                ModifiedOn = blog.ModifiedOn,
                Id = blog.Id,
                Summary = blog.Summary,
                Title = blog.Title,
                RecentBlogs = recentBlogs,
                CommentsCount = commentCount,
                Comments = comments,
                SubComments = subcomments,
                BlogsPerCategory = allBlogs
            };

            return View(blogModel);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id");
            ViewData["Categories"] = new SelectList(_context.Options.Where(x => x.OptionGroupId == 1), "Id", "Name");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Summary,Body,AuthorId,CategoryId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Blog blog, IFormFile thumbnail)
        {
            if (ModelState.IsValid)
            {
                IFormFile uploadedThumbnailImage;

                MemoryStream ms = new MemoryStream();
                Guid ImageID = new Guid();

                if (thumbnail != null)
                {
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
                    else
                    {
                        ThumbnailTempError();
                        FileTypeErrorThumbnail();
                        ViewData["CategoryId"] = new SelectList(_context.Options, "ID", "Title", blog.CategoryId);
                        return View();
                    }

                    ImageID = imageEntity.Id;
                }

                //get author
                var author = _context.Authors.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                blog.ThumbnailId = ImageID;
                blog.Id = Guid.NewGuid();
                blog.AuthorId = author.Id;
                blog.CreatedBy = User.Identity.Name;
                blog.CreatedOn = DateTime.Now;
                blog.ModifiedBy = User.Identity.Name;
                blog.ModifiedOn = DateTime.Now;
                _context.Add(blog);
                await _context.SaveChangesAsync();

                //once post is created, send out emails
                //foreach (var item in _context.Subscribers.ToList())
                //{
                    await emailSenderService.ExecuteEmailsList(_context.Subscribers.ToList(),String.Format("New Blog Post: {0}", blog.Title), blog.Body, "Holy Ghost Fire House");
                //}
                //after sending emails, go back to home page
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", blog.AuthorId);
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", blog.AuthorId);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Summary,Body,AuthorId,CateoryId,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", blog.AuthorId);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(Guid id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }

        public void ThumbnailTempError()
        {
            TempData["ThumbnailErrorMessage"] = "Only files with the file extension JPEG, PNG, TIF are allowed";
        }

        public void FileTypeErrorThumbnail()
        {
            var errorMsg = TempData["SolutionErrorMessage"] as string;
            ViewBag.SolutionType = errorMsg;
            //ViewBag.SolutionType = "Only files with the file extension JPEG, PNG, TIF are allowed";
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

        /*=======================comment section===================*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddComment(BlogViewModel blog)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    BlogPostId = blog.Id,
                    Body = blog.Comment.Body,
                    CreatedBy = blog.Comment.Name,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = blog.Comment.Name,
                    ModifiedOn = DateTime.Now,
                    Name = blog.Comment.Name,
                    Id = Guid.NewGuid()
                };
                
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = blog.Id});
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", blog.AuthorId);
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddSubComment(BlogViewModel blog, Guid commentId)
        {
            if (ModelState.IsValid)
            {

                var comment = _context.Comments.Where(x => x.Id == commentId).FirstOrDefault();
                var subcomment = new SubComment()
                {
                    BlogPostId = comment.BlogPostId,
                    Body = blog.Comment.Body,
                    CreatedBy = blog.Comment.Name,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = blog.Comment.Name,
                    ModifiedOn = DateTime.Now,
                    Name = blog.Comment.Name,
                    Id = Guid.NewGuid(),
                    CommentId = commentId
                    
                };

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = comment.BlogPostId });
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", blog.AuthorId);
            return View(blog);
        }
    }
}
