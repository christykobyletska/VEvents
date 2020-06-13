using VEvents.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VEvents.Poller
{
    public class Worker : BackgroundService
    {
        private readonly TimeSpan _workerPeriod = TimeSpan.FromSeconds(15);

        private readonly ILogger<Worker> _logger;
        private readonly IEventPoller _eventPoller;
        private readonly VEventsDbContext _VEventsDbContext;

        public Worker(ILogger<Worker> logger, IEventPoller eventPoller, VEventsDbContext VEventsDbContext)
        {
            _logger = logger;
            _eventPoller = eventPoller;
            _VEventsDbContext = VEventsDbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    var events = await _eventPoller.DoPollAsync(stoppingToken).ConfigureAwait(false);

                    var existingEventTitles = await _VEventsDbContext.Events
                        .Where(e => e.DateAndTime >= DateTime.Now)
                        .Select(e => e.Title)
                        .ToListAsync(stoppingToken)
                        .ConfigureAwait(false);

                    var newEvents = events
                        .Where(e => !existingEventTitles.Any(t=>t.Equals(e.Title, StringComparison.InvariantCultureIgnoreCase)))
                        .ToList();

                    if (newEvents.Any())
                    {
                        await _VEventsDbContext.Events.AddRangeAsync(newEvents, stoppingToken).ConfigureAwait(false);
                        await _VEventsDbContext.SaveChangesAsync(stoppingToken).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Polling round failed with {ex}");
                }

                await Task.Delay(_workerPeriod, stoppingToken);
            }
        }
    }
}
