using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RcsLogic;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Services;
using RcsLogic.Watchdog;

namespace Tests
{
    public class WatchdogTests : IStoreManagementClient
    {
        private bool _taskFailReceived = false;
        private bool _toteReturned = false;
        private ToteRepository _toteRepository;
        private RoutingService _routingService;
        private LocationService _locationService;
        private LocationRepository _locationRepository;
        private ILoggerFactory _loggerFactory;
        private TaskBundleService _taskBundleService;
        private IServiceProvider _serviceProvider;
        private IPlcService _plcService;
        private const string _mockBarcode = "0000001";

        [Test]
        public void WatchdogExecutorTest()
        {
            var watchdogExecutor = new WatchdogExecutor(_loggerFactory.CreateLogger<WatchdogExecutor>());
            var watchdogMock = new Mock<IWatchdog>();
            watchdogMock.Setup(t => t.Execute());
            watchdogExecutor.RegisterWatchdog(watchdogMock.Object);
            
            Thread.Sleep(30000);
            
            watchdogMock.Verify(mock => mock.Execute());
        }
        
        [Test]
        public void TestTimedOutedMoveTask()
        {
            _taskBundleService.Add(new TaskBundle()
            {
                tasks = new List<TaskBase>()
                {
                    new MoveTask()
                    {
                        processingStartedDate = DateTime.Now,
                        taskId = new TaskId("lorem_ipsum_task_id"),
                        taskStatus = RcsTaskStatus.Idle,
                        lastUpdateDate = DateTime.Now,
                        toteId = "ToteId1"
                    }
                },
                creationDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10).Add(TimeSpan.FromSeconds(1))),
                taskBundleId = new TaskBundleId("lorem_ipsum")
            });

            var configMock = new Mock<IConfiguration>();

            var taskWatchdog = new TaskBundleWatchdog(configMock.Object, _loggerFactory, _taskBundleService, GetReturnToteHandler());
            
            taskWatchdog.Execute();
            
            if(_taskFailReceived && _toteReturned) Assert.Pass();
            Assert.Fail();
        }
        
        [Test]
        public void TestNotTimedOutedPickTask()
        {
            _taskBundleService.Add(new TaskBundle()
            {
                tasks = new List<TaskBase>()
                {
                    new PickTask()
                    {
                        processingStartedDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10).Add(TimeSpan.FromSeconds(1))),
                        taskId = new TaskId("lorem_ipsum_task_id"),
                        taskStatus = RcsTaskStatus.Idle,
                        lastUpdateDate = DateTime.Now,
                        sourceTote = new PickToteData()
                        {
                            toteId = "SourceToteId1",
                            slotId = 1
                        },
                        destTote = new PickToteData()
                        {
                            toteId = "DestToteId1",
                            slotId = 1
                        }
                    }
                },
                creationDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10).Add(TimeSpan.FromSeconds(1))),
                taskBundleId = new TaskBundleId("lorem_ipsum")
            });

            var configMock = new Mock<IConfiguration>();
            var taskWatchdog = new TaskBundleWatchdog(configMock.Object, _loggerFactory, _taskBundleService, GetReturnToteHandler());
            
            taskWatchdog.Execute();

            if(_taskFailReceived) Assert.Fail();
            Assert.Pass();
        }
        
        [Test]
        public void TestTimedOutToteOnConveyor()
        {
            var assignedLocation = _locationRepository.GetLocationByFunction(LocationFunction.Pick);
            var assignedStorageLocation = _locationRepository.GetLocationByFunction(LocationFunction.Storage);
            
            CreateTote(assignedLocation, assignedStorageLocation);

            var configMock = new Mock<IConfiguration>();
            var toteWatchdog = new ToteLocationWatchdog(configMock.Object, _loggerFactory, _toteRepository, 
                GetReturnToteHandler(), _routingService, _locationService, _locationRepository, _plcService );
            
            toteWatchdog.Execute();

            if(!_toteReturned || !_toteRepository.GetToteByBarcode(_mockBarcode)
                .storageLocation.Equals(assignedStorageLocation)) Assert.Fail();
            Assert.Pass();
        }
        
        [Test]
        public void TestTimedOutToteWithoutLocation()
        {
            Location assignedLocation = null;
            var assignedStorageLocation = _locationRepository.GetLocationByFunction(LocationFunction.Storage);
            
            CreateTote(assignedLocation, assignedStorageLocation);

            var configMock = new Mock<IConfiguration>();
            var smMock = new Mock<IStoreManagementClient>();
            smMock.Setup(t => t.ToteNotification(It.IsAny<Tote>(), It.IsAny<Location>(), It.IsAny<ToteRotation>(),
                It.IsAny<ToteStatus>()));
            var toteWatchdog =
                new ToteLocationUnknownWatchdog(configMock.Object, _loggerFactory, _toteRepository, smMock.Object);
            
            toteWatchdog.Execute();

            smMock.Verify(mock => mock.ToteNotification(It.IsAny<Tote>(), It.IsAny<Location>(), It.IsAny<ToteRotation>(), ToteStatus.LocationUnknown), Times.Once);
            if(_toteRepository.GetToteByBarcode(_mockBarcode).status != ToteStatus.LocationUnknown) Assert.Fail();
            Assert.Pass();
        }
        
        [Test]
        public void TestTimedOutToteOnLoadingGate()
        {
            var assignedLocation = _locationRepository.GetLocationByFunction(LocationFunction.Pick);
            var assignedStorageLocation = _locationRepository.GetLocationByFunction(LocationFunction.LoadingGate);
            
            CreateTote(assignedLocation, assignedStorageLocation);
            
            var configMock = new Mock<IConfiguration>();
            var toteWatchdog = new ToteLocationWatchdog(configMock.Object, _loggerFactory, _toteRepository, 
                GetReturnToteHandler(), _routingService, _locationService, _locationRepository, _plcService );
            
            toteWatchdog.Execute();

            if(!_toteReturned) Assert.Fail("Tote not returned");
            if(_toteRepository.GetToteByBarcode(_mockBarcode)
            .storageLocation.Equals(assignedStorageLocation)) Assert.Fail("Storage location not changed");
            if(_toteRepository.GetToteByBarcode(_mockBarcode)
                .storageLocation.zone.function != LocationFunction.Technical) 
                Assert.Fail("Tote not on technical location");
                
            Assert.Pass();
        }

        private void CreateTote(Location assignedLocation, Location assignedStorageLocation)
        {
            _toteRepository.Add(new Tote()
            {
                id = 1,
                toteBarcode = _mockBarcode,
                lastLocationUpdate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10)),
                locationId = assignedLocation?.id,
                status = ToteStatus.Ready,
                storageLocationId = assignedStorageLocation.id,
                typeId = _toteRepository.GetToteType(ToteHeight.high, TotePartitioning.bipartite).id
            });
        }

        private IReturnToteHandler GetReturnToteHandler()
        {
            var returnToteHandler = new Mock<IReturnToteHandler>();
            returnToteHandler.Setup(m => m.ReturnTote(It.IsAny<string>()))
                .Callback<string>(it =>
                {
                    Console.WriteLine($"Returning tote: {it}");
                    _toteReturned = true;
                });
            return returnToteHandler.Object;
        }

        [Test]
        public void TestTimedOutedPickTask()
        {
            _taskBundleService.Add(new TaskBundle()
            {
                tasks = new List<TaskBase>()
                {
                    new PickTask()
                    {
                        processingStartedDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10).Add(TimeSpan.FromSeconds(1))),
                        taskId = new TaskId("lorem_ipsum_task_id"),
                        taskStatus = RcsTaskStatus.Executing,
                        lastUpdateDate = DateTime.Now,
                        sourceTote = new PickToteData()
                        {
                            toteId = "SourceToteId1",
                            slotId = 1
                        },
                        destTote = new PickToteData()
                        {
                        toteId = "DestToteId1",
                        slotId = 1
                    }
                    }
                },
                creationDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10).Add(TimeSpan.FromSeconds(1))),
                taskBundleId = new TaskBundleId("lorem_ipsum")
            });
            
            var configMock = new Mock<IConfiguration>();
            var taskWatchdog = new TaskBundleWatchdog(configMock.Object, _loggerFactory, _taskBundleService, GetReturnToteHandler());
            taskWatchdog.Execute();
            
            if(_taskFailReceived && _toteReturned) Assert.Pass();
            Assert.Fail();
        }

        [TearDown]
        public void TearDown()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            _dbContext.Database.EnsureDeleted();
            Thread.Sleep(10);
        }

        [SetUp]
        public void SetupContext()
        {
            _serviceProvider = new ServiceCollection()
                .AddDbContext<StoreDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: "StoreDatabase");
                })
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("Default", LogLevel.Information);
                }).BuildServiceProvider();
            StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();
            _dbContext.Database.EnsureCreated();
            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            _taskBundleService = new TaskBundleService(_loggerFactory, this, null, null, null, null);
            _toteRepository = new ToteRepository(_serviceProvider);
            _locationRepository = new LocationRepository(_serviceProvider);
            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            _routingService = new RoutingService(null, _serviceProvider, _locationRepository, deviceStatusService);
            _locationService = new LocationService(_loggerFactory, _locationRepository, _toteRepository, _routingService);
            _plcService = new PlcServiceMock(_loggerFactory, new ScanNotificationListenerRegistry(_loggerFactory),
                new TransferRequestDoneListenerRegistry(_loggerFactory),  new PickRequestDoneListenerRegistry(_loggerFactory));
            
            _dbContext.Dispose();
        }

        public void ReportTaskState(TaskBase task, int? picked = null, int? failed = null, FailReason? failReason = null,
            string failDescription = null)
        {
            if(task.taskStatus == RcsTaskStatus.Faulted) _taskFailReceived = true;
        }

        public void ToteNotification(Tote tote, Location scanLocation, ToteRotation toteRotation,
            ToteStatus toteStatus)
        {
            throw new NotImplementedException();
        }

        private void WaitForFailReceived()
        {
            var failTime = DateTime.Now.AddSeconds(5);
            while (!_taskFailReceived && !_toteReturned && DateTime.Now < failTime)
            {
                Thread.Sleep(10);
            }
            Thread.Sleep(10);
        }

        public ToteData GetToteDetails(string toteBarcode)
        {
            throw new NotImplementedException();
        }
    }
}