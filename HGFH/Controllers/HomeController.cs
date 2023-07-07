using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HGFH.Models;
using HGFH.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using HGFH.Services;

namespace HGFH.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IEmailSenderService _senderService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IEmailSender emailSender, IEmailSenderService senderService)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
            _senderService = senderService;
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

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Blogs.Include(b => b.Author).OrderByDescending(x=> x.CreatedOn).Take(3);
            var info = headerInfo();
            var index = new IndexViewModel()
            {
                Blogs = await applicationDbContext.ToListAsync(),
                YearlyDeclarationBlog = info.YearlyDeclarationBlog,
                MonthylDeclarationBlog = info.MonthylDeclarationBlog

            };
           // return View(await applicationDbContext.ToListAsync());
            return View(index);
        }

        public async Task<IActionResult> About()
        {
            var info = headerInfo();

            var index = new IndexViewModel()
            {
                YearlyDeclarationBlog = info.YearlyDeclarationBlog,
                MonthylDeclarationBlog = info.MonthylDeclarationBlog

            };
            return View(index);
        }

        public IActionResult Ministries()
        {
            var info = headerInfo();

            var index = new IndexViewModel()
            {
                YearlyDeclarationBlog = info.YearlyDeclarationBlog,
                MonthylDeclarationBlog = info.MonthylDeclarationBlog

            };
            return View("Ministries", index);
        }

        public async Task<IActionResult> Sermons()
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
            return View("Sermons",index);
        }

        public IActionResult ContactUs()
        {
            return View("ContactUs");
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                //send email to the person who submitted the email
                await _senderService.ExecuteEmails(
               contact.Subject,
               $@"Good day.
                <br/>Thank you for contacting Holy Ghost Fire House. We will reach out to you through the details you have provided us. 
                <br/>Should you feel like it is urgent, please reach out to us through telephone on (+27) 13 752 2076.

                <br/>Regards,
                <br/>Holy Ghost Fire House
                ",contact.EmailAddress,"NoReply");

                // send email to the info account 
                var message = $@"
                Good day
                <br/>
                This email comes from {contact.Name} regarding the issue described below. Email address to contact is :{contact.EmailAddress}
                <br/>
                <br/>
                The message: <strong>{contact.Message}</strong>
                 ";
                await _senderService.ExecuteEmails("HGFH Website - "+contact.Subject,message, "info@hgfh.org", "ContactUs-Website");

                return RedirectToAction("ContactUs");
            }

            return View("ContactUs");
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

        public async Task<IActionResult> Subscribe(Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                subscriber.Id = Guid.NewGuid();
                subscriber.ModifiedBy = "System";
                subscriber.CreatedBy = "System";
                subscriber.CreatedOn = DateTime.Now;
                subscriber.ModifiedOn = DateTime.Now;
                _context.Add(subscriber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index");
        }
    }
}
