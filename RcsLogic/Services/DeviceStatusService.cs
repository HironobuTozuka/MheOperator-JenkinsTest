using System;
using System.Collections.Generic;
using System.Linq;
using Common.Exceptions;
using Microsoft.Extensions.Configuration;
using RcsLogic.Models.Device;

namespace RcsLogic.Services
{
    public class DeviceStatusService
    {
        private List<DeviceStatus> _deviceStatuses = new List<DeviceStatus>();

        public DeviceStatusService(IConfiguration configuration)
        {
            var statuses = configuration.GetSection("DeviceStatusService")?.GetChildren();
            if (statuses == null) return;
            foreach (var configurationSection in statuses)
            {
                var deviceId = new DeviceId(configurationSection.Key);
                Status status;
                if(!Status.TryParse(configurationSection.Value, out status)) throw new ConfigurationError(){configurationSection = configurationSection};
                _deviceStatuses.Add(new DeviceStatus(deviceId, status));
            }
        }

        public bool IsEnabled(DeviceId deviceId)
        {
            var status = _deviceStatuses.FirstOrDefault(st => st.DeviceId.Equals(deviceId));
            return status == null || status?.Status == Status.Enabled;
        }
        
    }
}