﻿using Locations.API.Model;
using Microsoft.BuildingBlocks.EventBus.Events;
using System.Collections.Generic;

namespace Locations.API.IntegrationEvents.Events
{
    public class UserLocationUpdatedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public List<UserLocationDetails> LocationList { get; set; }

        public UserLocationUpdatedIntegrationEvent(string userId, List<UserLocationDetails> locationList)
        {
            UserId = userId;
            LocationList = locationList;
        }
    }
}
