using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Common.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Common.Models;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using MheOperator.StoreManagementApi.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using RcsLogic.Models;
using Newtonsoft.Json;
using RcsLogic.Services;

namespace Tests
{
    public class LocationServiceTests : DeviceTests
    {
        private new ILogger<RoutingTests> _logger;


        [SetUp]
        public void Setup()
        {
            base.Setup(false, false, false, false, false, false);

            _logger = _loggerFactory.CreateLogger<RoutingTests>();
        }

        [Test]
        public void TestCreatingToteByToteServiceAndReassigning()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();

            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            RoutingService router =
                new RoutingService(_serviceProvider.GetRequiredService<IMemoryCache>(), 
                    _serviceProvider, _serviceProvider.GetRequiredService<LocationRepository>(), deviceStatusService);
            var locationService = new LocationService(_loggerFactory, _locationRepository, _toteRepository, router);
            var toteService = new ToteService(_loggerFactory, _toteRepository, _locationRepository, _storeManagementClient);
            toteService.SaveTote(new ScanNotificationModel()
            {
                LocationId = "LOAD1_2",
                ToteBarcode = "LoremIpsum",
                ToteRotation = ToteRotation.normal,
                ToteType = new RequestToteType()
                {
                    ToteHeight = ToteHeight.high,
                    TotePartitioning = TotePartitioning.bipartite
                }
            });

            var taskBundle = new TaskBundle()
            {
                creationDate = DateTime.Now,
                isInternal = false,
                taskBundleId = new TaskBundleId(Guid.NewGuid().ToString()),
                tasks = new List<TaskBase>()
                {
                    new MoveTask()
                    {
                        destZone = new ZoneId("CHILL"),
                        isInternal = false,
                        lastUpdateDate = DateTime.Now,
                        processingStartedDate = DateTime.Now,
                        taskId = new TaskId(Guid.NewGuid().ToString()),
                        taskStatus = RcsTaskStatus.Idle,
                        toteId = "LoremIpsum"
                    }
                }
            };
            
            locationService.AssignDestLocationsToTaskBundle(taskBundle);

            var assignedLocation = ((MoveTask) (taskBundle.tasks.First())).destLocation;
            
            if(!assignedLocation.zone.id.Equals("CHILL")) Assert.Fail();

            var tote = _toteRepository.GetToteByBarcode("LoremIpsum");

            var internalMoveTask = _taskBundles.GetInternalMoveTask(tote, tote.storageLocation);
            
            if(!internalMoveTask.destLocation.Equals(assignedLocation)) Assert.Fail();
            
            locationService.AssignDestLocationsToTaskBundle(_taskBundles.GetTaskBundle(internalMoveTask.taskId));
            
            if(!internalMoveTask.destLocation.Equals(assignedLocation)) Assert.Fail();
            
            Assert.Pass();
        }
        
        [Test]
        public void TestCreatingToteByToteServiceAndRaisingToteLocationNotFound()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();

            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            RoutingService router =
                new RoutingService(_serviceProvider.GetRequiredService<IMemoryCache>(), 
                    _serviceProvider, _serviceProvider.GetRequiredService<LocationRepository>(), deviceStatusService);
            var locationService = new LocationService(_loggerFactory, _locationRepository, _toteRepository, router);
            var toteService = new ToteService(_loggerFactory, _toteRepository, _locationRepository, _storeManagementClient);
            string error = "";
            toteService.SaveTote(new ScanNotificationModel()
            {
                LocationId = "LOAD1_2",
                ToteBarcode = "LoremIpsum",
                ToteRotation = ToteRotation.normal,
                ToteType = new RequestToteType()
                {
                    ToteHeight = ToteHeight.high,
                    TotePartitioning = TotePartitioning.bipartite
                }
            });

            var taskBundle = new TaskBundle()
            {
                creationDate = DateTime.Now,
                isInternal = false,
                taskBundleId = new TaskBundleId(Guid.NewGuid().ToString()),
                tasks = new List<TaskBase>()
                {
                    new MoveTask()
                    {
                        destZone = new ZoneId("AMBIENT"),
                        isInternal = false,
                        lastUpdateDate = DateTime.Now,
                        processingStartedDate = DateTime.Now,
                        taskId = new TaskId(Guid.NewGuid().ToString()),
                        taskStatus = RcsTaskStatus.Idle,
                        toteId = "LoremIpsum"
                    }
                }
            };
            try
            {
                locationService.AssignDestLocationsToTaskBundle(taskBundle);
            }
            catch (SmHttpControllerException ex)
            {
                var code = ApiErrorFactory.CreateApiError(ex);
                error = JsonConvert.SerializeObject(code);
            }
            
            if(error.IsNullOrEmpty()) Assert.Fail();
            Assert.Pass();
        }
        
        [Test]
        public void GetClosestLocationFrom()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();

            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            RoutingService router =
                new RoutingService(_serviceProvider.GetRequiredService<IMemoryCache>(), 
                    _serviceProvider, _serviceProvider.GetRequiredService<LocationRepository>(), deviceStatusService);
            var locationService = new LocationService(_loggerFactory, _locationRepository, _toteRepository, router);

            var location = _locationRepository.GetLocationByPlcId("A2L0406");

            var checkLocation = locationService.GetClosestLocationOfFunction(location, LocationFunction.Crane);
            var nextLocation = router.GetNextLocation(location, checkLocation);
            
            if(nextLocation.plcId.Equals("CA_P1")) Assert.Pass();
            
            Assert.Fail();
        }

       [Test]
        public void GetLocationFromStorage()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "B", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "A", _dbContext, minCol: 7);
            MockUtils.AddToteToDB("0000003", _dbContext.toteTypes.First(), "A", _dbContext, minCol: 2);
            _dbContext.Dispose();

            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            RoutingService router =
                new RoutingService(_serviceProvider.GetRequiredService<IMemoryCache>(),
                    _serviceProvider, _serviceProvider.GetRequiredService<LocationRepository>(), deviceStatusService);
            var locationService = new LocationService(_loggerFactory, _locationRepository, _toteRepository, router);
            var toteService = new ToteService(_loggerFactory, _toteRepository, _locationRepository, _storeManagementClient);
            toteService.SaveTote(new ScanNotificationModel()
            {
                LocationId = "LOAD1_2",
                ToteBarcode = "LoremIpsum",
                ToteRotation = ToteRotation.normal,
                ToteType = new RequestToteType()
                {
                    ToteHeight = ToteHeight.high,
                    TotePartitioning = TotePartitioning.bipartite
                }
            });
            var tote = _toteRepository.GetToteByBarcode("LoremIpsum");

            var location = locationService._FindLocation(new ZoneId("STORAGE"), tote);

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