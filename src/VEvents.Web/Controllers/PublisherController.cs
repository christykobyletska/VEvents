using VEvents.Web.Data;
using VEvents.Web.Extensions;
using VEvents.Web.Models;
using VEvents.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VEvents.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PublisherController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var publishers = new List<User>();

            foreach (var appUser in _userManager.Users)
            {
                var isAdmin = await _userManager.IsInRoleAsync(appUser, Roles.Admin);
                if (isAdmin)
                {
                    continue;
                }

                publishers.Add(appUser.ToUserViewModel());
            }

            return View(publishers);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher.ToUserViewModel());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Email,PhoneNumber,Password")] User publisher)
        {
            if (ModelState.IsValid)
            {
                publisher.Id = Guid.NewGuid().ToString().ToLowerInvariant();

                var identityUser = publisher.ToUserModel();

                await _userManager.CreateAsync(identityUser, publisher.Password);

                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Users.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher.ToUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FullName,Email,PhoneNumber")] User publisher)
        {
            ModelState["Password"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.Users.First(u => u.Id == id);

                    user.FullName = publisher.FullName;
                    user.Email = publisher.Email;
                    user.PhoneNumber = publisher.PhoneNumber;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id))
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
            return View(publisher);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher.ToUserViewModel());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var publisher = await _context.Users.FindAsync(id);
            _context.Users.Remove(publisher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
