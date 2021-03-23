﻿using Microsoft.Extensions.Logging;

namespace MG.WebHost.Infrastructure.Contracts
{
    public static class EventIdFactory
    {
        public static EventId ConsumerError => new EventId(1234);
    }
}