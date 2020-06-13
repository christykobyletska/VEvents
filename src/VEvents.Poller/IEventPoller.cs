using VEvents.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VEvents.Poller
{
    public interface IEventPoller
    {
        public Task<IEnumerable<Event>> DoPollAsync(CancellationToken cancellationToken);
    }
}
