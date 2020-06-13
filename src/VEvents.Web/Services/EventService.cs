using VEvents.Data.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.IO;

namespace VEvents.Web.Services
{
    public class EventService : IEventService
    {
        private readonly string _VEventsApiUrl;

        public EventService(IConfiguration configuration)
        {
            _VEventsApiUrl = configuration.GetConnectionString("VEventsApiUrl");
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await GetEventsAsync("all");
        }

        public async Task<IEnumerable<Event>> GetActualEventsAsync()
        {
            return await GetEventsAsync("actual");
        }

        public async Task<IEnumerable<Event>> GetUserEventsAsync(string userId)
        {
            return await GetEventsAsync($"user/{userId}");
        }

        public async Task<Event> GetEventAsync(string id)
        {
            Event @event;

            using var client = new HttpClient {BaseAddress = new Uri($"{_VEventsApiUrl}/{id}")};

            var result = await client.GetAsync(string.Empty);

            if (result.IsSuccessStatusCode)
            {
                @event = await result.Content.ReadAsAsync<Event>();
            }
            else
            {
                throw new HttpRequestException($"An error occured while getting event with Id:{id}.");
            }

            return @event;
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            using var client = new HttpClient {BaseAddress = new Uri(_VEventsApiUrl)};

            var result = await client.PostAsJsonAsync(string.Empty, @event);

            if (result.IsSuccessStatusCode)
            {
                @event = await result.Content.ReadAsAsync<Event>();
            }
            else
            {
                throw new HttpRequestException($"An error occured while creating event with Title:{@event.Title}.");
            }

            return @event;
        }

        public async Task EditEventAsync(string id, Event @event)
        {
            using var client = new HttpClient {BaseAddress = new Uri($"{_VEventsApiUrl}/{id}") };

            var result = await client.PutAsJsonAsync(string.Empty, @event);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occured while updating event with Id:{@event.Id}.");
            }
        }

        public async Task DeleteEventAsync(string id)
        {
            using var client = new HttpClient {BaseAddress = new Uri($"{_VEventsApiUrl}/{id}") };

            var result = await client.DeleteAsync(string.Empty);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occured while deleting event with Id:{id}.");
            }
        }

        private async Task<IEnumerable<Event>> GetEventsAsync(string routePart)
        {
            IEnumerable<Event> events;

            using var client = new HttpClient {BaseAddress = new Uri($"{_VEventsApiUrl}/{routePart}")};

            var result = await client.GetAsync(string.Empty);

            if (result.IsSuccessStatusCode)
            {
                events = await result.Content.ReadAsAsync<IEnumerable<Event>>();
            }
            else
            {
                throw new HttpRequestException($"An error occured while getting '{routePart}' events.");
            }

            return events;
        }
    }
}
