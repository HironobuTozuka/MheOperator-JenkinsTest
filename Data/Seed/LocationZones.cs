using System.Collections.Generic;
using Common.Models.Location;

namespace Data.Seed
{
    public static class LocationZones
    {
        public static List<Zone> Seed()
        {
            var zones = new List<Zone>();
            zones.Add(new Zone()
            {
                id = "CONVEYOR", function = LocationFunction.Conveyor, temperatureRegime = TemperatureRegime.Any
            });
            zones.Add(new Zone()
            {
                id = "CRANE", function = LocationFunction.Crane, temperatureRegime = TemperatureRegime.Any
            });
            zones.Add(new Zone()
            {
                id = "PLACE", function = LocationFunction.Place, temperatureRegime = TemperatureRegime.Any
            });
            zones.Add(new Zone()
            {
                id = "PICK", function = LocationFunction.Pick, temperatureRegime = TemperatureRegime.Any
            });
            zones.Add(new Zone()
            {
                id = "ORDER_GATE_1", function = LocationFunction.OrderGate, temperatureRegime = TemperatureRegime.Any,
                plcGateId = "OrderGate1"
            });
            zones.Add(new Zone()
            {
                id = "ORDER_GATE_2", function = LocationFunction.OrderGate, temperatureRegime = TemperatureRegime.Any,
                plcGateId = "OrderGate2"
            });
            zones.Add(new Zone()
            {
                id = "LOADING_GATE", function = LocationFunction.LoadingGate, temperatureRegime = TemperatureRegime.Any,
                plcGateId = "LoadingGate"
            });
            zones.Add(new Zone()
            {
                id = "LOADING_GATE_CNV", function = LocationFunction.Conveyor, temperatureRegime = TemperatureRegime.Any
            });
            zones.Add(new Zone()
            {
                id = "AMBIENT", function = LocationFunction.Storage, temperatureRegime = TemperatureRegime.Ambient
            });
            zones.Add(new Zone()
            {
                id = "CHILL", function = LocationFunction.Storage, temperatureRegime = TemperatureRegime.Chill
            });
            zones.Add(new Zone()
            {
                id = "STAGING", function = LocationFunction.Staging, temperatureRegime = TemperatureRegime.Ambient
            });
            zones.Add(new Zone()
            {
                id = "TECHNICAL", function = LocationFunction.Technical, temperatureRegime = TemperatureRegime.Any
            });
            return zones;
        }
    }
}