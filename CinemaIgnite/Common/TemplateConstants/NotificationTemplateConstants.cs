﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TemplateConstants
{
    public class NotificationTemplateConstants
    {
        public const string MovieProjectionTitle = "Coming soon";
        public const string TicketBoughtTitle = "Ticket bought";
        public const string ProjectionCancelledTitle = "Projection cancelled";

        public const string MovieProjectionMessage = "A projection for the {0} is available on {1} at {2}";
        public const string TicketBoughtMessage = "You've successfully bought a ticket for {0} on {1} at {2}";
        public const string ProjectionCancelledMessage = "Unfortunately, the projection of {0} on {1} at {2} has been cancelled. You will get your ticket refunded in a few days";
    }
}