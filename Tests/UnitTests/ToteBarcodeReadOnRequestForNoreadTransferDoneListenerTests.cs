using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
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
using RcsLogic.RcsController;
using RcsLogic.Services;

namespace Tests
{
    public class ToteBarcodeReadOnRequestForNoreadTransferDoneListenerTests
    {
        private IServiceProvider _serviceProvider;
        private ILoggerFactory _loggerFactory;
        private ToteRepository _toteRepository;
        private LocationRepository _locationRepository;
        private DeviceRegistry _deviceRegistry;
        private ToteBarcodeReadOnRequestForNoReadTransferDoneListener _toteBarcodeReadOnRequestForNoReadTransferDoneListener;
        private readonly Transfer _tranferToReturn = new Transfer();

        [Test]
        public void TestMoveCompleteOfNoreadThatWasScannedOnCrane()
        {

            var location =
                _locationRepository.GetLocationByZone(_locationRepository.GetZoneById(new ZoneId("TECHNICAL")));
            var sourceLocation =
                _locationRepository.GetLocationByPlcId("CNV2_1");
            const ToteStatus toteStatus = ToteStatus.Ready;
            const string readBarcode = "0000001";
            const string noreadBarcode = "NOREAD";
            var tote = CreateTote(readBarcode, location, toteStatus);
            var toteNoread = CreateTote(noreadBarcode, location, toteStatus);
            
            _toteRepository.Add(tote);
            
            var task = CreateTask(location, noreadBarcode);

            UpdateTransferToReturn(location, sourceLocation, task, toteNoread);

            SimulateTransferDone(readBarcode, location, sourceLocation);

            var toteAfterTransfer = _toteRepository.GetTotesWithBarcodeContaining("NOREAD").FirstOrDefault();
            if(toteAfterTransfer != null) Assert.Fail("No read tote created unnecessarily"); 
            toteAfterTransfer = _toteRepository.GetToteByBarcode(tote.toteBarcode);
            if(toteAfterTransfer?.status != ToteStatus.NoRead) Assert.Fail("Tote status in not noread");
            Assert.Pass();
        }
        

        private void SimulateTransferDone(string toteBarcode, Location location, Location sourceLocation)
        {
            _toteBarcodeReadOnRequestForNoReadTransferDoneListener.ProcessTransferRequestDone(new TransferRequestDoneModel(
                new ToteTransferRequestDoneModel()
                {
                    sourceToteBarcode = toteBarcode,
                    actualDestLocationId = location.plcId,
                    requestedDestLocationId = location.plcId,
                    sourceLocationId = sourceLocation.plcId,
                    sortCode = 1,
                },
                null
            ));
        }

        private void UpdateTransferToReturn(Location location, Location sourceLocation, MoveTask task, Tote tote)
        {
            _tranferToReturn.destLocation = location;
            _tranferToReturn.sourceLocation = sourceLocation;
            _tranferToReturn.status = Transfer.RequestStatus.Execute;
            _tranferToReturn.task = task;
            _tranferToReturn.tote = tote;
        }

        private static MoveTask CreateTask(Location location, string toteBarcode)
        {
            return new MoveTask()
            {
                destLocation = location,
                destZone = location.zone.zoneId,
                isInternal = false,
                lastUpdateDate = DateTime.Now,
                processingStartedDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(1)),
                taskId = new TaskId("lorem_ipsum"),
                taskStatus = RcsTaskStatus.Executing,
                toteId = toteBarcode
            };
        }

        private Tote CreateTote(string toteBarcode, Location location, ToteStatus toteStatus)
        {
            var tote = new Tote()
            {
                id = 1,
                toteBarcode = toteBarcode,
                lastLocationUpdate = DateTime.Now.Subtract(TimeSpan.FromMinutes(10)),
                locationId = location.id,
                status = toteStatus,
                storageLocationId = location.id,
                typeId = _toteRepository.GetToteType(ToteHeight.high, TotePartitioning.bipartite).id
            };
            return tote;
        }

        [TearDown]
        public void TearDown()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            _dbContext.Database.EnsureDeleted();
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
            
            _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>().Database.EnsureCreated();
            
            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            _toteRepository = new ToteRepository(_serviceProvider);
            var servicedLocationProvider = new ServicedLocationProvider(_serviceProvider, _loggerFactory);
            _locationRepository = new LocationRepository(_serviceProvider);
            var config = new Mock<IConfiguration>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            var routingService = new RoutingService(null, _serviceProvider, _locationRepository, deviceStatusService);
            var transferDeviceMock = new Mock<ITransferCompletingDevice>();
            transferDeviceMock.Setup(t => t.DeviceId)
                .Returns(new DeviceId("CA_P1"));
            transferDeviceMock.Setup(t => t.ShouldHandleTransferDone(It.IsNotNull<ToteTransferRequestDoneModel>()))
                .Returns(true);
            transferDeviceMock.Setup(t => t.GetCompletedTransfer(It.IsNotNull<ToteTransferRequestDoneModel>()))
                .Returns(_tranferToReturn);

            var deviceInitializerMock = new Mock<IDeviceInitializer>();
            deviceInitializerMock.Setup(t => t.GetDevices()).Returns(new List<IDevice>(){transferDeviceMock.Object});
            _deviceRegistry = new DeviceRegistry(servicedLocationProvider, routingService, deviceInitializerMock.Object, _loggerFactory);
            var locationService = new LocationService(_loggerFactory, _locationRepository, _toteRepository, routingService);
            
            _toteBarcodeReadOnRequestForNoReadTransferDoneListener = new ToteBarcodeReadOnRequestForNoReadTransferDoneListener(
                _loggerFactory, locationService, _deviceRegistry, _toteRepository, _locationRepository);
            
        }

    }
}