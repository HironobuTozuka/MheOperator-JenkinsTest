using Common.Models;
using Common.Models.Plc;
using Common.Models.Tote;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using RcsLogic;
using RcsLogic.Models.Device;
using RcsLogic.Services;


namespace Tests
{
    public class RoutingIntegrationTests : IntegrationTestBase
    {
        [Test]
        public void TestRouteFromLoad1_2ToRacking()
        {
            //Thread.Sleep(2000);
            var routingService =
                new RoutingService(ServiceProvider.GetRequiredService<IMemoryCache>(), 
                    ServiceProvider, ServiceProvider.GetRequiredService<LocationRepository>(),
                    ServiceProvider.GetRequiredService<DeviceStatusService>());
            var locationRepository = ServiceProvider.GetRequiredService<LocationRepository>();
            var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<RoutingIntegrationTests>();

            var zoneController = ServiceProvider.GetRequiredService<RcsInitializer>();

            zoneController.ScanNotificationListenerRegistry.NotifyListeners(new ScanNotificationModel()
            {
                LocationId = "LOAD1_4",
                ToteBarcode = Barcode.NoRead,
                ToteRotation = ToteRotation.unknown,
                ToteType = new RequestToteType()
                {
                    TotePartitioning = TotePartitioning.unknown,
                    ToteHeight = ToteHeight.unknown
                }
            });

            logger.LogInformation("trtrtrtrt");
        }
    }
}