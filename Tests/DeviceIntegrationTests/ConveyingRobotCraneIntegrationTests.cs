using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IO;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Microsoft.EntityFrameworkCore.Internal;
using RcsLogic.Models;

namespace Tests
{
    public class ConveyingRobotCraneIntegrationTests : DeviceTests
    {
        int robotPickRecieved = 0;
        int robotPlaceRecieved = 0;
        int deliveredToCrane = 0;
        int toDeliverToCrane = 0;
        int _totesReleased;

        [SetUp]
        public void Setup()
        {
            base.Setup(true, true, true, true, true, true);
            _logger = _loggerFactory.CreateLogger<ConveyingRobotCraneIntegrationTests>();

            robotPickRecieved = 0;
            robotPlaceRecieved = 0;
            deliveredToCrane = 0;
            _totesReleased = 0;

            _logger.LogInformation("Test preparation complete");
        }

        [Test]
        public void TestTransferAndPickFromCNV1_3ToCNV3_2DestToteOnRackB()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "B", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "A", _dbContext, minCol: 7);
            MockUtils.AddToteToDB("0000003", _dbContext.toteTypes.First(), "A", _dbContext, minCol: 2);
            toDeliverToCrane = 2;
            //toDeliverToCrane = 1;

            _dbContext.Dispose();

            var taskBundle =
                MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV3_2", "RPP1", 1, 2, _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000003", "0000001", "CNV4_2", "RPP1", 1, 1, _serviceProvider);

            _taskBundles.Add(taskBundle);

           // taskBundle = MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV3_2", "CNV1_5", 1, 2, _serviceProvider);

            var totesToRelease = _taskBundles.GetFirstPickTaskBundle().tasks.OfType<PickTask>()
                .Select(task => task.sourceTote.toteId).Distinct().Count();

           // _taskBundles.Add(taskBundle);

            int _partsToPick = taskBundle.tasks.OfType<PickTask>().Select(task => task.quantity).Sum();


            WaitForTransfersComplete();
            WaitForPickingComplete(_partsToPick);

            taskBundle = MockUtils.MockMoveTaskBundle("0000001", "ORDER1", _serviceProvider);
            _taskBundles.Add(taskBundle);

            WaitForTotesOnRack();

            if (taskBundle.tasks.Any(task => task.taskStatus != RcsTaskStatus.Complete))
            {
                var guids = taskBundle.tasks.Where(task => task.taskStatus != RcsTaskStatus.Complete)
                    .Select(task => task.taskId).ToList();
                _logger.LogWarning("Not Completed Tasks: {0}", guids);
                Assert.Fail("Not all tasks are complete!");
            }

            if (_totesReleased != totesToRelease) Assert.Fail("Not all totes are released!");
            if (deliveredToCrane != toDeliverToCrane) Assert.Fail("Pick totes ware not delivered to crane");
            _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            if (_dbContext.totes.Any(tote => tote.location.zone.function != LocationFunction.Storage
                                             && tote.location.zone.function != LocationFunction.Staging))
            {
                string totesNotOnRack = "";
                _dbContext.totes.Where(tote => tote.location.zone.function != LocationFunction.Storage
                                               && tote.location.zone.function != LocationFunction.Staging)
                    .ToList()
                    .ForEach(tote => totesNotOnRack += tote.toteBarcode + ", ");
                Assert.Fail("Not all totes on rack {0}", totesNotOnRack);
            }

            Assert.Pass();
        }

        [Test]
        public void TestScanOnRPP1()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "B", _dbContext);
            var toteType = _dbContext.toteTypes.First();

            _dbContext.Dispose();
            
            
            PlcService.MockScanNotification("RPP1", "0000001", toteType);

            Thread.Sleep(100);

            
            if (_taskBundles.ToList().Any()) Assert.Fail("Task bundle was created");

            Assert.Pass();
        }
        private void WaitForTransfersComplete()
        {
            var failTime = DateTime.Now.AddSeconds(9999);
            while (!(robotPickRecieved>0 && robotPlaceRecieved>0 && deliveredToCrane == toDeliverToCrane) &&
                   DateTime.Now < failTime)
            {
                Thread.Sleep(10);
            }

            Thread.Sleep(50);
        }

        private void WaitForPickingComplete(int partsToPick)
        {
            var failTime = DateTime.Now.AddSeconds(9999);
            while (((StoreManagementMock) _storeManagementClient).partsPicked != partsToPick && DateTime.Now < failTime)
            {
                Thread.Sleep(10);
            }
        }

        private void WaitForTotesOnRack()
        {
            var failTime = DateTime.Now.AddSeconds(9999);
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            while ((_dbContext.totes.Any(tote => tote.location == null)
                    || _dbContext.totes.Any(tote => (tote.location.zone.function != LocationFunction.Storage
                                                     && tote.location.zone.function !=
                                                     LocationFunction.Staging))) &&
                   DateTime.Now < failTime)
            {
                _dbContext.Dispose();
                Thread.Sleep(50);
                _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            }

            Thread.Sleep(50);
        }

        public override void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            if (scanNotification.LocationId == "CNV2_2")
            {
                deliveredToCrane += 1;
            }
            if( robotPickRecieved>1) PlcService.returnedRobotSortCode = 1;
            if (scanNotification.LocationId == "CNV3_2" || scanNotification.LocationId == "CNV4_2" || scanNotification.LocationId == "CNV2_5")
                robotPickRecieved += 1;
            if (scanNotification.LocationId == "RPP1")
                robotPlaceRecieved += 1;
        }

        public override void ToteReady(PrepareForPicking prepareForPicking)
        {
            TaskBundle taskBundle = _taskBundles.GetFirstPickTaskBundle();

            _logger.LogInformation("Ready for picking sent to robot for {1} on location {2}", prepareForPicking.Tote.toteBarcode,
                prepareForPicking.Location.plcId);
            if (prepareForPicking.Location.plcId == ((PickTask) taskBundle?.tasks.First())?.sourceTote.pickLocation.plcId)
                robotPickRecieved += 1;
            if (prepareForPicking.Location.plcId == ((PickTask) taskBundle?.tasks.First())?.destTote.pickLocation.plcId)
                robotPlaceRecieved += 1;
        }

        public override void ReturnTote(string toteBarcode)
        {
            _totesReleased += 1;
        }
    }
}