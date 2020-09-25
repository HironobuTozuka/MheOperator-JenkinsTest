using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using Moq;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Common.Models;
using System.Threading;
using Common.Models.Location;
using Common.Models.Task;
using Microsoft.Extensions.Caching.Memory;
using RcsLogic.Models;
using RcsLogic.Robot;

namespace Tests
{
    public class ConveyingRobotIntegrationTests : DeviceTests
    {
        int robotPickRecieved = 0;
        int robotPlaceRecieved = 0;
        int deliveredToCrane = 0;
        int _totesReleased;
        private List<PickTask> _pickTasks;
        private bool failOnlyFirstPick = false;

        [SetUp]
        public void Setup()
        {
            base.Setup(true, false, false, true, false, false);
            _logger = _loggerFactory.CreateLogger<ConveyingTests>();

            robotPickRecieved = 0;
            robotPlaceRecieved = 0;
            deliveredToCrane = 0;
            _totesReleased = 0;
            failOnlyFirstPick = false;

            _logger.LogInformation("Test preparation complete");
        }


        [Test]
        public void TestTransferAndPickFromCNV1_3ToCNV3_2DestToteOnRackB()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "B", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000003", _dbContext.toteTypes.First(), "A", _dbContext);

            var toteType = _dbContext.toteTypes.First();
            _dbContext.Dispose();

            var taskBundle =
                MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV3_2", "RPP1", 1, 2, _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000003", "0000001", "CNV4_2", "RPP1", 1, 1,
                _serviceProvider);

            _pickTasks = taskBundle.tasks.OfType<PickTask>().ToList();
            _taskBundles.Add(taskBundle);

            int _partsToPick = taskBundle.tasks.OfType<PickTask>().Select(task => task.quantity).Sum();

            var totesToRelease = _taskBundles.GetFirstPickTaskBundle().tasks.OfType<PickTask>()
                .Select(task => task.sourceTote.toteId).Distinct().Count();

            PlcService.MockScanNotification("CNV1_3", "0000002", toteType);
            PlcService.MockScanNotification("RPP1", "0000001", toteType);
            PlcService.MockScanNotification("CNV1_3", "0000003", toteType);

            WaitForPickingComplete(_partsToPick);
            WaitForTransfersComplete();
            

            if (_taskBundles.ToList().Where(task => !task.isInternal).ToList().Count > 0) 
                Assert.Fail("Not all tasks are complete!");
            if (_totesReleased != totesToRelease) Assert.Fail("Not all totes are released!");
            if (deliveredToCrane<2) Assert.Fail("Pick totes ware not delivered to crane");
            Assert.Pass();
        }
        
        [Test]
        public void TestNoMoreSourceNotEmptyOnOnePick()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "B", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000003", _dbContext.toteTypes.First(), "A", _dbContext);

            PlcService.returnedRobotSortCode = RobotSortCodes.FinishedNoMoreTargetsNotEmpty.Code;
            
            var toteType = _dbContext.toteTypes.First();
            _dbContext.Dispose();

            var taskBundle =
                MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV3_2", "RPP1", 1, 2, _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000003", "0000001", "CNV4_2", "RPP1", 1, 1,
                _serviceProvider);

            _pickTasks = taskBundle.tasks.OfType<PickTask>().ToList();
            _taskBundles.Add(taskBundle);
            failOnlyFirstPick = true;

            int _partsToPick = taskBundle.tasks.OfType<PickTask>().Select(task => task.quantity).Sum();

            var totesToRelease = _taskBundles.GetFirstPickTaskBundle().tasks.OfType<PickTask>()
                .Select(task => task.sourceTote.toteId).Distinct().Count();

            PlcService.MockScanNotification("CNV1_3", "0000002", toteType);
            PlcService.MockScanNotification("RPP1", "0000001", toteType);
            PlcService.MockScanNotification("CNV1_4", "0000003", toteType);

            
            WaitForPickingComplete(_partsToPick);
            WaitForTransfersComplete();
            

            if (_taskBundles.ToList().Where(task => !task.isInternal).ToList().Count > 0) 
                Assert.Fail("Not all tasks are complete!");
            if (_totesReleased != totesToRelease) Assert.Fail("Not all totes are released!");
            if (deliveredToCrane<2) Assert.Fail("Pick totes ware not delivered to crane");
            if(_pickTasks.Any(task => task.taskStatus != RcsTaskStatus.Complete)) 
                Assert.Fail("Not all tasks are complete");
            Assert.Pass();
        }
        
        [Test]
        public void TestNoMoreSourceNotEmpty()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "B", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000003", _dbContext.toteTypes.First(), "A", _dbContext);

            PlcService.returnedRobotSortCode = RobotSortCodes.FinishedNoMoreTargetsNotEmpty.Code;
            
            var toteType = _dbContext.toteTypes.First();
            _dbContext.Dispose();

            var taskBundle =
                MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV3_2", "RPP1", 1, 2, _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000003", "0000001", "CNV4_2", "RPP1", 1, 1,
                _serviceProvider);

            _pickTasks = taskBundle.tasks.OfType<PickTask>().ToList();
            _taskBundles.Add(taskBundle);

            int _partsToPick = taskBundle.tasks.OfType<PickTask>().Select(task => task.quantity).Sum();

            var totesToRelease = _taskBundles.GetFirstPickTaskBundle().tasks.OfType<PickTask>()
                .Select(task => task.sourceTote.toteId).Distinct().Count();

            PlcService.MockScanNotification("CNV1_3", "0000002", toteType);
            PlcService.MockScanNotification("RPP1", "0000001", toteType);
            PlcService.MockScanNotification("CNV1_4", "0000003", toteType);

            WaitForPickingComplete(_partsToPick);
            WaitForTransfersComplete();
            

            if (_taskBundles.ToList().Where(task => !task.isInternal).ToList().Count > 0) 
                Assert.Fail("Not all tasks are complete!");
            if (_totesReleased != totesToRelease) Assert.Fail("Not all totes are released!");
            if (deliveredToCrane<2) Assert.Fail("Pick totes ware not delivered to crane");
            if(_pickTasks.Any(task => task.taskStatus != RcsTaskStatus.Faulted)) Assert.Fail("Not all tasks are faulted");
            Assert.Pass();
        }

        public override void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            if (scanNotification.LocationId == "CNV2_2")
            {
                deliveredToCrane += 1;
            }
            if(failOnlyFirstPick && robotPickRecieved>1) PlcService.returnedRobotSortCode = 1;
            if (scanNotification.LocationId == "CNV3_2" || scanNotification.LocationId == "CNV4_2" || scanNotification.LocationId == "CNV2_5")
                robotPickRecieved += 1;
            if (scanNotification.LocationId == "RPP1")
                robotPlaceRecieved += 1;
        }


        private void WaitForTransfersComplete()
        {
            var failTime = DateTime.Now.AddSeconds(30);
            while (!(robotPickRecieved>0 && robotPlaceRecieved>0 && deliveredToCrane>=2) && DateTime.Now < failTime)
            {
                Thread.Sleep(10);
            }
        }

        private void WaitForPickingComplete(int partsToPick)
        {
            MockUtils.WaitFor(() => ((StoreManagementMock) _storeManagementClient).partsPicked == partsToPick ||
            _pickTasks.All(task => task.taskStatus == RcsTaskStatus.Complete || task.taskStatus == RcsTaskStatus.Faulted) );
        }

        public override void ToteReady(PrepareForPicking prepareForPicking)
        {
            TaskBundle taskBundle = _taskBundles.GetFirstPickTaskBundle();

            _logger.LogInformation("Ready for picking sent to robot for {1} on location {2}", prepareForPicking.Tote.toteBarcode,
                prepareForPicking.Location.plcId);

        }

        public override void ReturnTote(string toteBarcode)
        {
            _totesReleased += 1;
        }
    }
}