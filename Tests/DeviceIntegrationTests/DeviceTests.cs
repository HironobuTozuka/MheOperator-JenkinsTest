using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Models;
using Common.Models.Plc;
using Data;
using MheOperator.StoreManagementApi.Controllers;
using Moq;
using PlcRequestQueueService;
using RcsLogic;
using RcsLogic.Crane;
using RcsLogic.Gates;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.RcsController;
using RcsLogic.RcsController.Recovery;
using RcsLogic.RcsController.ToteCommand;
using RcsLogic.Robot;
using RcsLogic.Services;
using RcsLogic.Watchdog;

namespace Tests
{
    public abstract class DeviceTests : IScanNotificationListener, IPrepareForPickingDevice,
        IReturnToteHandler
    {
        protected PlcServiceMock PlcService;
        protected TaskBundleService _taskBundles;
        protected ILoggerFactory _loggerFactory;
        protected IServiceProvider _serviceProvider;
        protected ILogger<DeviceTests> _logger;
        protected IStoreManagementClient _storeManagementClient;
        protected LocationRepository _locationRepository;
        protected ToteRepository _toteRepository;
        protected TotesReadyForPicking _totesReadyForPicking;
        protected ushort returnedRobotSortCode
        {
            get => PlcService.returnedRobotSortCode;
            set => PlcService.returnedRobotSortCode = value;
        }

        public abstract void ProcessScanNotification(ScanNotificationModel scanNotification);
        public abstract void ToteReady(PrepareForPicking prepareForPicking);
        public abstract void ReturnTote(string toteBarcode);

        public void Setup(bool enableConveyor, bool enableCraneA, bool enableCraneB, bool enableRobot,
            bool enableLoadingGate, bool enableOrderGate)
        {
            DeviceInitializerMock.EnableRobot = enableRobot;
            DeviceInitializerMock.EnableCraneA = enableCraneA;
            DeviceInitializerMock.EnableCraneB = enableCraneB;
            DeviceInitializerMock.EnableLoadingGate = enableLoadingGate;
            DeviceInitializerMock.EnableConveyor = enableConveyor;
            var mock = new Mock<IPrepareForPickingDevice>();
            mock.Setup(t => t.ToteReady(It.IsNotNull<PrepareForPicking>())).Callback<PrepareForPicking>(ToteReady);
            
            _serviceProvider = new ServiceCollection().AddDbContext<StoreDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: "StoreDatabase");
                })
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("Default", LogLevel.Trace);
                }).AddSingleton<Data.LocationRepository>()
                .AddSingleton<Data.ToteRepository>()
                .AddSingleton<TaskBundleController>()
                .AddSingleton<IMujinClient, MujinClientMock>()
                .AddSingleton<IStoreManagementClient, StoreManagementMock>()
                .AddSingleton<TaskBundleService>()
                .AddSingleton<TaskBundleWatchdog>()
                .AddSingleton<LocationService>()
                .AddSingleton<ToteService>()
                .AddSingleton(new Mock<IConfiguration>().Object)
                .AddSingleton<RoutingService>()
                .AddSingleton<DeviceStatusService>()
                .AddSingleton<TotesReadyForPicking>()
                .AddSingleton<RcsController>()
                .AddSingleton<DeviceRegistry>()
                .AddSingleton<IDeviceInitializer, DeviceInitializerMock>()
                .AddSingleton(mock.Object)
                .AddSingleton<ServicedLocationProvider>()
                .AddSingleton<IPlcService, PlcServiceMock>()
                .AddSingleton<LocationStatus>()
                .AddSingleton<RcsInitializer>()
                .AddSingleton<TransferRequestDoneWatcher>()
                .AddSingleton<ToteLocationWatchdog>()
                .AddSingleton<CraneIdleListener>()
                .AddSingleton<MoveTaskCompletingScanNotificationListener>()
                .AddSingleton<ToteLocationUpdatingScanNotificationListener>()
                .AddSingleton<ToteCommandDecisionTree>()
                .AddSingleton<UnknownToteRouter>()
                .AddSingleton<RecoveryHandler>()
                .AddSingleton<TransferRequestDoneListenerRegistry>()
                .AddSingleton<PickRequestDoneListenerRegistry>()
                .AddSingleton<ScanNotificationListenerRegistry>()
                .AddSingleton<PlcNotificationListenerRegistry>()
                .AddSingleton<WatchdogExecutor>()
                .AddSingleton<ToteLocationUpdatingScanNotificationListener>()
                .AddSingleton<ToteLocationWatchdog>()
                .AddSingleton<ToteLocationUnknownWatchdog>()
                .AddSingleton<StoreManagementNotifyingTransferDoneListener>()
                .AddSingleton<ToteBarcodeReadOnRequestForNoReadTransferDoneListener>()
                .AddSingleton<IReturnToteHandler, RcsController>()
                .AddSingleton(new Mock<IKafkaConsumerGroup>().Object)
                .AddMemoryCache()
                .AddSingleton(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build())
                .BuildServiceProvider();
            using StoreDbContext dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();
            dbContext.Database.EnsureCreated();
            _toteRepository = _serviceProvider.GetRequiredService<ToteRepository>();
            _locationRepository = _serviceProvider.GetRequiredService<LocationRepository>();

            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();

            _logger = _loggerFactory.CreateLogger<DeviceTests>();

            _storeManagementClient = _serviceProvider.GetRequiredService<IStoreManagementClient>();

            _totesReadyForPicking = _serviceProvider.GetRequiredService<TotesReadyForPicking>();

            _taskBundles = _serviceProvider.GetRequiredService<TaskBundleService>();

            PlcService = _serviceProvider.GetRequiredService<IPlcService>() as PlcServiceMock;
            var deviceRegistry = _serviceProvider.GetRequiredService<DeviceRegistry>();

            var scanNotificationListenerRegistry =
                _serviceProvider.GetRequiredService<ScanNotificationListenerRegistry>();
            
            scanNotificationListenerRegistry.RegisterListener(this);

            _serviceProvider.GetRequiredService<TaskBundleService>();
            
            _serviceProvider.GetRequiredService<RcsInitializer>();
            
            if (enableRobot)
            {
                deviceRegistry.GetDevicesOfType<RobotDevice>().ForEach(rd => rd.RegisterReturnHandler(this));
            }
            
            PlcService.Subscribe(new CraneIdleListener(_loggerFactory,  _serviceProvider.GetRequiredService<RcsController>(), deviceRegistry));
        }

        [TearDown]
        public void TearDown()
        {
            using var dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
            PlcService.ResetPickRequestConfRed();
            try
            {
                ((ServiceProvider)_serviceProvider).Dispose();
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning("TaskWasCancelled", ex);
                try
                {
                    ((ServiceProvider)_serviceProvider).Dispose();
                }
                catch (TaskCanceledException ex2)
                {
                    _logger.LogWarning("TaskWasCancelled", ex2);
                }
                
            }
            Thread.Sleep(10);
            
        }

        private class DeviceInitializerMock : IDeviceInitializer
        {
            private List<IDevice> _devices = new List<IDevice>();
            public static bool EnableRobot = true;
            public static bool EnableCraneA = true;
            public static bool EnableCraneB = true;
            public static bool EnableLoadingGate = true;
            public static bool EnableConveyor = true;

            public DeviceInitializerMock(ILoggerFactory loggerFactory,
                IPlcService plcService,
                TaskBundleService taskBundleService,
                ToteRepository toteRepository,
                LocationRepository locationRepository,
                LocationService locationService,
                IMujinClient mujinClient,
                IStoreManagementClient storeManagementClient,
                LocationStatus locationStatus,
                TotesReadyForPicking totesReadyForPicking,
                ServicedLocationProvider servicedLocationProvider,
                RoutingService routingService,
                IConfiguration configuration,
                IPrepareForPickingDevice prepareForPickingDevice)
            {
                if(EnableCraneA)
                    _devices.Add( new CraneDevice(new DeviceId("CA_P"), plcService, loggerFactory, CraneType.DoubleShelved, taskBundleService,
                    routingService, storeManagementClient,  toteRepository, locationRepository,
                    locationService, locationStatus, totesReadyForPicking));
                else
                {
                    var device = new Mock<ITransferDevice>();
                    device.Setup(it => it.DeviceId).Returns(new DeviceId("CA_P"));
                    _devices.Add(device.Object);
                }
                if(EnableCraneB)
                    _devices.Add(  new CraneDevice(new DeviceId("CB_P"), plcService, loggerFactory, CraneType.SingleShelved, taskBundleService,
                    routingService, storeManagementClient,  toteRepository, locationRepository,
                    locationService, locationStatus, totesReadyForPicking));
                else
                {
                    var device = new Mock<ITransferDevice>();
                    device.Setup(it => it.DeviceId).Returns(new DeviceId("CB_P"));
                    _devices.Add(device.Object);
                }
                if(EnableLoadingGate)
                    _devices.Add( new LoadingGate(new DeviceId("LOAD1"), plcService, loggerFactory, taskBundleService,
                    servicedLocationProvider, toteRepository, routingService));
                else
                {
                    var device = new Mock<ITransferDevice>();
                    device.Setup(it => it.DeviceId).Returns(new DeviceId("LOAD1"));
                    _devices.Add(device.Object);
                }
                _devices.Add(  new OrderGates(new DeviceId("ORDER"), plcService, loggerFactory, taskBundleService, servicedLocationProvider, configuration));
                if(EnableConveyor)
                {
                    _devices.Add(new ConveyorDevice(new DeviceId("CNV"), plcService, loggerFactory, taskBundleService,
                        locationStatus, totesReadyForPicking));
                }
                if (EnableRobot)
                {
                    _devices.Add(  new RobotDevice(new DeviceId("Mujin"), configuration, plcService, loggerFactory, taskBundleService,
                        mujinClient, totesReadyForPicking));
                }
                else
                {
                    _devices.Add(prepareForPickingDevice);
                }
            }
            public List<IDevice> GetDevices()
            {
                return _devices;
            }
        }


        public DeviceId DeviceId => new DeviceId("DeviceTests");
    }
}