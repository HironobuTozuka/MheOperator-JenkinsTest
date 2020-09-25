using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Common.Models;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Configuration;
using Moq;
using RcsLogic.Models;
using RcsLogic.RcsController;
using RcsLogic.Services;

namespace Tests
{
    public class RoutingTests : DeviceTests
    {
        private new ILogger<RoutingTests> _logger;


        [SetUp]
        public void Setup()
        {
            base.Setup(false, false, false, false, false, false);

            _logger = _loggerFactory.CreateLogger<RoutingTests>();
        }

        [Test]
        public void TestRoutingFromCNV1_5ToORDER1()
        {
            TestRouting("CNV1_5", "ORDER1");
        }
        
        [Test]
        public void TestRoutingFromCNV1_3ToA1L0702()
        {
            TestRouting("CNV1_3", "A1L0702");
        }

        [Test]
        public void TestRoutingFromCNV1_5ToORDER2()
        {
            TestRouting("CNV1_5", "ORDER2");
        }

        [Test]
        public void TestRoutingFromCNV1_5ToB1L0311F()
        {
            TestRouting("CNV1_5", "B1L0311F");
        }

        [Test]
        public void TestRoutingFromA1L0310ToB1L0311F()
        {
            TestRouting("A1L0310", "B1L0311F");
        }

        [Test]
        public void TestRoutingFromCNV1_5ToA1L0309()
        {
            TestRouting("CNV1_5", "A1L0309");
        }

        [Test]
        public void TestRoutingFromLOAD1_2ToA1L0308()
        {
            TestRouting("LOAD1_2", "A1L0308");
        }

        [Test]
        public void TestRoutingFromLOAD1_2ToB1L0311F()
        {
            TestRouting("LOAD1_2", "B1L0311F");
        }

        [Test]
        public void TestRoutingFromCA_P1toCA_P1()
        {
            TestRouting("CA_P1", "CA_P1");
        }
        
        [Test]
        public void TestRoutingFromB2L0105FToB2L0305F()
        {
            TestRouting("B2L0105F", "B2L0305F");
        }

        [Test]
        public void TestRoutingFromCNV1_5ToLOAD1_2()
        {
            TestRouting("CNV1_5", "LOAD1_2");
        }
        
        [Test]
        public void TestRouteCost()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();

            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            RoutingService router =
                new RoutingService(_serviceProvider.GetRequiredService<IMemoryCache>(), 
                    _serviceProvider, _serviceProvider.GetRequiredService<LocationRepository>(), deviceStatusService);
            
            var start = _dbContext.locations.First(location => location.plcId == "LOAD1_2");
            var end = _dbContext.locations.First(location => location.plcId == "CA_P1");
            
            var routeCost = router.GetRouteCost(start, end);
            
            if(routeCost == 3) Assert.Pass();
            Assert.Fail();
        }

        public void TestRouting(string startLoc, string endLoc)
        {
            try
            {
                var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                    .GetRequiredService<StoreDbContext>();

                var config = new Mock<IConfiguration>();
                var deviceStatusService = new DeviceStatusService(config.Object);
                RoutingService router =
                    new RoutingService(_serviceProvider.GetRequiredService<IMemoryCache>(), 
                        _serviceProvider, _serviceProvider.GetRequiredService<LocationRepository>(), deviceStatusService);

                var start = _dbContext.locations.First(location => location.plcId == startLoc);
                var end = _dbContext.locations.First(location => location.plcId == endLoc);

                var testRunTimeTime = DateTime.Now.AddSeconds(2);
                var startTime = DateTime.Now;
                var runTime = DateTime.Now.Subtract(startTime);

                var currentLocation = start;
                currentLocation = router.GetNextLocation(currentLocation, end);


                _logger.LogInformation("Current location: {0}", currentLocation.plcId);

                while (currentLocation.plcId != end.plcId && DateTime.Now < testRunTimeTime)
                {
                    startTime = DateTime.Now;
                    currentLocation = router.GetNextLocation(currentLocation, end);
                    runTime = DateTime.Now.Subtract(startTime);
                    _logger.LogInformation("Route found, execution time {0}s {1}ms:", runTime.Seconds,
                        runTime.Milliseconds);
                    _logger.LogInformation("Current location: {0}", currentLocation.plcId);
                }

                if (currentLocation.plcId != end.plcId) Assert.Fail("Not routed to dest");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            Assert.Pass();
        }
        
        [Test]
        public void TestUnknownToteRouter()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();

            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            RoutingService router =
                new RoutingService(_serviceProvider.GetRequiredService<IMemoryCache>(), 
                    _serviceProvider, _serviceProvider.GetRequiredService<LocationRepository>(), deviceStatusService);
            
            var locationService = new LocationService(_loggerFactory, _locationRepository, _toteRepository, router);
            
            var unknownToteRouter = new UnknownToteRouter(_loggerFactory, _locationRepository, router, locationService, _taskBundles);

            var transfer = unknownToteRouter.RequestTransfer("LOAD1_4", "UNKNOWN",
                new RequestToteType() {ToteHeight = ToteHeight.unknown, TotePartitioning = TotePartitioning.unknown});
            
            if(transfer.destLocation.zone.function == LocationFunction.Crane) Assert.Pass();
            Assert.Fail();
        }

        public override void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
        }

        public override void ToteReady(PrepareForPicking prepareForPicking)
        {
            throw new NotImplementedException();
        }

        public override void ReturnTote(string toteBarcode)
        {
            throw new NotImplementedException();
        }
    }
}