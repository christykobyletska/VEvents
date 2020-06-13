using VEvents.Web.Data;
using VEvents.Web.Extensions;
using VEvents.Web.Models;
using VEvents.Web.Services;
using VEvents.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VEvents.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEventService _eventService;

        public EventController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, IEventService eventService)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var actualEvents = await _eventService.GetActualEventsAsync(currentUserId).ConfigureAwait(false);

            return View(actualEvents
                .Select(e => e.ToEventViewModel(_applicationDbContext))
                .OrderBy(e => e.DateAndTime));
        }

        [Authorize]
        public async Task<IActionResult> Manage()
        {
            IEnumerable<VEvents.Data.Models.Event> events;

            if (User.IsInRole(Roles.Admin))
            {
                events = await _eventService.GetAllEventsAsync().ConfigureAwait(false);
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                events = await _eventService.GetUserEventsAsync(userId).ConfigureAwait(false);
            }

            return View(events.Select(e => e.ToEventViewModel(_applicationDbContext)));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _eventService.GetEventAsync(id).ConfigureAwait(false);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event.ToEventViewModel(_applicationDbContext));
        }

        [Authorize]
        public async Task<IActionResult> AllDetails(string id) => await Details(id);

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Details,DateAndTime,Status")] Event @event)
        {
            if (!ModelState.IsValid) return View(@event);

            @event.Publisher = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            await _eventService.CreateEventAsync(@event.ToEventModel()).ConfigureAwait(false);

            return RedirectToAction(nameof(Manage));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _eventService.GetEventAsync(id).ConfigureAwait(false);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event.ToEventViewModel(_applicationDbContext));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Title,Details,DateAndTime,Status")] Event @event)
        {
            if (!ModelState.IsValid) return View(@event);

            await _eventService.EditEventAsync(id, @event.ToEventModel()).ConfigureAwait(false);

            return RedirectToAction(nameof(Manage));
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _eventService.GetEventAsync(id).ConfigureAwait(false);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event.ToEventViewModel(_applicationDbContext));
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _eventService.DeleteEventAsync(id);

            return RedirectToAction(nameof(Manage));
        }

        [Authorize]
        public async Task<IActionResult> ToggleLike(string id)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _eventService.ToggleLikeAsync(id, currentUserId);

            return RedirectToAction(nameof(Index));
        }
    }
}
