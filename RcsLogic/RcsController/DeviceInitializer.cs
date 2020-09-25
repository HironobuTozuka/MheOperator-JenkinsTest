using System.Collections.Generic;
using Common;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RcsLogic.Crane;
using RcsLogic.Gates;
using RcsLogic.Models.Device;
using RcsLogic.Robot;
using RcsLogic.Services;

namespace RcsLogic.RcsController
{
    public class DeviceInitializer : IDeviceInitializer
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IPlcService _plcService;
        private readonly TaskBundleService _taskBundleService;
        private readonly ToteRepository _toteRepository;
        private readonly LocationRepository _locationRepository;
        private readonly LocationService _locationService;
        private readonly IMujinClient _mujinClient;
        private readonly IStoreManagementClient _storeManagementClient;
        private readonly LocationStatus _locationStatus;
        private readonly TotesReadyForPicking _totesReadyForPicking;
        private readonly ServicedLocationProvider _servicedLocationProvider;
        private readonly RoutingService _routingService;
        private readonly IConfiguration _configuration;

        public DeviceInitializer(ILoggerFactory loggerFactory,
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
            IConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            _plcService = plcService;
            _taskBundleService = taskBundleService;
            _toteRepository = toteRepository;
            _locationRepository = locationRepository;
            _locationService = locationService;
            _mujinClient = mujinClient;
            _storeManagementClient = storeManagementClient;
            _locationStatus = locationStatus;
            _totesReadyForPicking = totesReadyForPicking;
            _servicedLocationProvider = servicedLocationProvider;
            _routingService = routingService;
            _configuration = configuration;
        }

        public List<IDevice> GetDevices()
        {
            return new List<IDevice>
            {
                new RobotDevice(new DeviceId("Mujin"), _configuration, _plcService, _loggerFactory, _taskBundleService, _mujinClient, _totesReadyForPicking),
                new CraneDevice(new DeviceId("CA_P"), _plcService, _loggerFactory, CraneType.DoubleShelved, _taskBundleService,
                    _routingService, _storeManagementClient, _toteRepository, _locationRepository,
                    _locationService, _locationStatus, _totesReadyForPicking),
                new CraneDevice(new DeviceId("CB_P"), _plcService, _loggerFactory, CraneType.SingleShelved, _taskBundleService,
                    _routingService, _storeManagementClient, _toteRepository, _locationRepository,
                    _locationService, _locationStatus, _totesReadyForPicking),
                new ConveyorDevice(new DeviceId("CNV"), _plcService, _loggerFactory, _taskBundleService, _locationStatus, _totesReadyForPicking),
                new LoadingGate(new DeviceId("LOAD1"), _plcService, _loggerFactory, _taskBundleService
                    , _servicedLocationProvider, _toteRepository, _routingService),
                new OrderGates(new DeviceId("ORDER"), _plcService, _loggerFactory, _taskBundleService, _servicedLocationProvider, _configuration)
                
            };
        }
    }
}