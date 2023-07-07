using HGFH.Data;
using HGFH.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HGFH.Controllers
{
    public class BranchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext db)
        {
            _context = db;
        }
        public IActionResult HQ()
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
            return View("HQ", index);
        }
        public IActionResult Pretoria()
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
            return View("Pretoria", index);
        }
        public IActionResult Bushbuckridge()
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
            return View("Bushbuckridge",index);
        }
    }
}
