using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IO;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using RcsLogic;
using RcsLogic.Crane;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.RcsController;


namespace Tests
{
    public class ConveyingCraneTests : DeviceTests
    {
        bool robotPickRecieved = false;
        bool robotPlaceRecieved = false;
        bool arrivedAtLoadingGate = false;
        private ServicedLocationProvider _servicedLocationProvider;

        [SetUp]
        public void Setup()
        {
            base.Setup(true, true, true, false, false, false);
            _servicedLocationProvider = new ServicedLocationProvider(_serviceProvider, _loggerFactory);
            _logger = _loggerFactory.CreateLogger<ConveyingTests>();
            robotPickRecieved = false;
            robotPlaceRecieved = false;
            _logger.LogInformation("Test preparation complete");
        }


        [Test]
        public void TestTransferFromRackToRobotDestToteOnRackBSourceToteOnRackA()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            var location = _dbContext.locations.Include(loc => loc.frontLocation)
                .FirstOrDefault(loc => loc.isBackLocation);
            MockUtils.AddToteToDB("00000001", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("00000002", _dbContext.toteTypes.First(), "B", _dbContext, storageLocation: location);
            MockUtils.AddToteToDB("00000003", _dbContext.toteTypes.First(), "B", _dbContext,
                storageLocation: location.frontLocation);

            _dbContext.Dispose();

            _taskBundles.Add(MockUtils.MockPickTaskBundle("00000001", "00000002", "CNV3_2", "RPP1", 1, 1,
                _serviceProvider));

            WaitForTransfersComplete();

            if (robotPickRecieved && robotPlaceRecieved) Assert.Pass();
            else Assert.Fail();
        }

        [Test]
        public void TestTransferFromRackAToLoadingGate()
        {
            arrivedAtLoadingGate = false;

            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            _dbContext.Dispose();

            _taskBundles.Add(MockUtils.MockMoveTaskBundle("0000001", "LOAD1_2", _serviceProvider));

            WaitForLoadGateArrival();

            if (arrivedAtLoadingGate) Assert.Pass();
            else Assert.Fail();
        }
        
        [Test]
        public void StealingToteFromRobot()
        {

            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "A", _dbContext);
            _dbContext.Dispose();

            var tote = _toteRepository.GetToteByBarcode("0000001");
            var location =
                _locationRepository.GetLocationByZone(_locationRepository.GetZoneByFunction(LocationFunction.Place));
            _toteRepository.UpdateToteLocation(tote, location);
            
            _totesReadyForPicking.Add(new PrepareForPicking()
            {
                Location = tote.location,
                Tote = tote,
                ToteRotation = ToteRotation.normal
            });
            _totesReadyForPicking.Block(tote);

            _taskBundles.Add(MockUtils.MockMoveTaskBundle("0000002", "RPP1", _serviceProvider));
            
            MockUtils.WaitFor(() => _toteRepository.GetToteByBarcode("0000002")?.location?.plcId.Equals("CNV1_5") == true);
            
            Thread.Sleep(500);

            if (_toteRepository.GetToteByBarcode("0000002")?.location?.plcId.Equals("CNV1_5") == true) Assert.Pass();
            else Assert.Fail();
        }
        
        [Test]
        public void TestCraneTimeout()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            _dbContext.Dispose();

            PlcService.craneNoResponse = true;

            _taskBundles.Add(MockUtils.MockMoveTaskBundle("0000001", "LOAD1_2", _serviceProvider));
            var crane = _serviceProvider.GetRequiredService<DeviceRegistry>().GetDeviceByDeviceId(new DeviceId("CA_P")) as CraneDevice;
            
            MockUtils.WaitFor(() => crane?.CraneState.IsBusy() == true);
            
            if (crane?.CraneState.IsBusy() == false) Assert.Fail();
            
            MockUtils.WaitFor(() => crane?.CraneState.IsBusy() == false);

            if (crane?.CraneState.IsBusy() == false) Assert.Pass();
            else Assert.Fail();
        }
        
        [Test]
        public void TestTransferFromRackAToLoadingGateOfNoRead()
        {
            arrivedAtLoadingGate = false;

            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("NOREAD1", _dbContext.toteTypes.First(), "A", _dbContext);
            _dbContext.Dispose();

            _taskBundles.Add(MockUtils.MockMoveTaskBundle("NOREAD1", "LOAD1_2", _serviceProvider));

            WaitForLoadGateArrival();
            WaitFor(() => !_toteRepository.Any("NOREAD1"));

            if (arrivedAtLoadingGate 
                && ((StoreManagementMock)_storeManagementClient).recievedScanFromLoadingGate
                && _taskBundles.Count == 0
                && !_toteRepository.Any("NOREAD1")) Assert.Pass();
            else Assert.Fail();
        }
        
        [Test]
        public void TestTransferFromRackAToLoadingGateOfNoReadPlcLaterRead()
        {
            arrivedAtLoadingGate = false;

            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("NOREAD1", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            _dbContext.Dispose();

            PlcService.changeNoreadToReadOnTransferDone = true;
            PlcService.changeNoreadToReadOnScan = true;

            _taskBundles.Add(MockUtils.MockMoveTaskBundle("NOREAD1", "LOAD1_2", _serviceProvider));

            WaitForLoadGateArrival();
            WaitFor(() => !_toteRepository.Any("NOREAD1"));

            if (arrivedAtLoadingGate 
                && ((StoreManagementMock)_storeManagementClient).recievedScanFromLoadingGate
                && _taskBundles.Count == 0
                && !_toteRepository.Any("NOREAD1")) Assert.Pass();
            else Assert.Fail();
        }
        
        [Test]
        public void TestTransferFromRackAToLoadingGateOfReadPlcLaterNORead()
        {
            arrivedAtLoadingGate = false;

            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            _dbContext.Dispose();
            
            PlcService.changeReadToNoreadOnScan = true;

            _taskBundles.Add(MockUtils.MockMoveTaskBundle("0000001", "LOAD1_2", _serviceProvider));

            WaitForLoadGateArrival();

            if (arrivedAtLoadingGate 
                && ((StoreManagementMock)_storeManagementClient).recievedScanFromLoadingGate
                && _taskBundles.Count == 0) Assert.Pass();
            else Assert.Fail();
        }

        public override void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            _logger.LogInformation("Scan notification recieved! Location: {0}, tote: {1}", scanNotification.LocationId,
                scanNotification.ToteBarcode);
            if (scanNotification.LocationId == "LOAD1_2") arrivedAtLoadingGate = true;
        }

        private void WaitForTransfersComplete()
        {
            WaitFor(() => robotPickRecieved && robotPlaceRecieved);
        }

        private void WaitForLoadGateArrival()
        {
            WaitFor(() => arrivedAtLoadingGate 
                          && ((StoreManagementMock)_storeManagementClient).recievedScanFromLoadingGate 
                          &&  _taskBundles.Count == 0);
        }

        private void WaitFor(Func<Boolean> condition)
        {
            var failTime = DateTime.Now.AddSeconds(30);
            while ((condition() == false) && DateTime.Now < failTime)
            {
                
                Thread.Sleep(10);
            }
            
            Thread.Sleep(50);
        }

        public override void ToteReady(PrepareForPicking prepareForPicking)
        {
            TaskBundle taskBundle = _taskBundles.GetFirstPickTaskBundle();

            _logger.LogInformation("Ready for picking sent to robot for {1} on location {2}", prepareForPicking.Tote.toteBarcode,
                prepareForPicking.Location.plcId);
            if (prepareForPicking.Location.plcId == ((PickTask) (taskBundle?.tasks?.First()))?.sourceTote?.pickLocation.plcId)
                robotPickRecieved = true;
            if (prepareForPicking.Location.plcId == ((PickTask) (taskBundle?.tasks?.First()))?.destTote?.pickLocation.plcId)
                robotPlaceRecieved = true;
        }

        public override void ReturnTote(string toteBarcode)
        {
            throw new NotImplementedException();
        }
    }
}