using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HGFH.Data;
using HGFH.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HGFH.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(IdentityRole role)
        {
            if (role == null)
            {
                return NotFound();
            }
            var roleDeleted = await roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AssignRole()
        {
            ViewData["roles"] = roleManager.Roles.ToList();
            ViewData["users"] = _userManager.Users.ToList();

            return View("AssignRole");
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole([Bind("Username,RoleName")] UserRoleViewModel userRole)
        {
            if (string.IsNullOrEmpty(userRole.Username) || string.IsNullOrEmpty(userRole.RoleName))
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(userRole.Username);
            await _userManager.AddToRoleAsync(user, userRole.RoleName);
            return RedirectToAction(nameof(Index));
        }
    }
}
