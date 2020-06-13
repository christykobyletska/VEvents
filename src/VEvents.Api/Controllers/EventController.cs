using VEvents.Data;
using VEvents.Data.Enums;
using VEvents.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VEvents.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly VEventsDbContext _VEventsDbContext;

        public EventController(VEventsDbContext VEventsDbContext)
        {
            _VEventsDbContext = VEventsDbContext;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Event>> GetAll()
        {
            return await _VEventsDbContext.Events.ToListAsync();
        }

        [HttpGet("actual")]
        public async Task<IEnumerable<Event>> GetActual()
        {
            return await _VEventsDbContext.Events
                .Where(e => e.Status == EventStatus.Published && e.DateAndTime >= DateTime.Now)
                .ToArrayAsync();
        }

        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<Event>> GetForUser(string userId)
        {
            return await _VEventsDbContext.Events
                .Where(e => e.PublisherId == userId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public Task<Event> Get(string id)
        {
            return _VEventsDbContext.Events.FirstOrDefaultAsync(m => m.Id == id);
        }

        [HttpPost]
        public async Task<Event> Post(Event @event)
        {
            _VEventsDbContext.Events.Add(@event);
            await _VEventsDbContext.SaveChangesAsync();
            return @event;
        }

        [HttpPut("{id}")]
        public async Task Put(string id, Event @event)
        {
            var eventToEdit = await _VEventsDbContext.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (eventToEdit == null)
            {
                throw new ArgumentException($"Event with Id:{@event.Id} does not exist.");
            }

            eventToEdit.Title = @event.Title;
            eventToEdit.Details = @event.Details;
            eventToEdit.DateAndTime = @event.DateAndTime;
            eventToEdit.Status = @event.Status;

            await _VEventsDbContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var @event = await _VEventsDbContext.Events.FirstOrDefaultAsync(m => m.Id == id);
            _VEventsDbContext.Events.Remove(@event);
            await _VEventsDbContext.SaveChangesAsync();
        }
    }
}
