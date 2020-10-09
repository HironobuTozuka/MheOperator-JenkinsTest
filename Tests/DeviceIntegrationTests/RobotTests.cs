using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using Ductus.FluentDocker.Common;
using Microsoft.EntityFrameworkCore.Internal;
using RcsLogic.Models;
using RcsLogic.Robot;

namespace Tests
{
    public class RobotTests : DeviceTests
    {
        private new ILogger<RobotTests> _logger;
        int _totesReleased;

        [SetUp]
        public void Setup()
        {
            base.Setup(true, false, false, true, false, false);
            _logger = _loggerFactory.CreateLogger<RobotTests>();
            _totesReleased = 0;
        }


        [Test]
        public void TestPickingFromCNV4_2ToCNV1_5SingleSlot()
        {
            var taskBundle =
                    (MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV4_2", "RPP1", 1, 2, _serviceProvider));

            TestPicking(taskBundle);
        }
        
        [Test]
        public void TestDestToteNotDetectedError()
        {
            PlcService.returnedRobotSortCode = RobotSortCodes.FinishedPlaceContainerNotDetected.Code;
            var taskBundle =
                (MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV4_2", "RPP1", 1, 2, _serviceProvider));

            var totesToRelease = InitTest(taskBundle, out var partsToPick);

            WaitForPickingCompleteAndTotesReleasedOrErrorToSM(partsToPick, totesToRelease);

            var taskBundleToCheck = _taskBundles.GetFirstPickTaskBundle();
            if (taskBundleToCheck != null && taskBundleToCheck.tasks
                .Any(task => task.taskStatus != RcsTaskStatus.Complete && task.taskStatus != RcsTaskStatus.Faulted))
                Assert.Fail("Not all tasks from task bundle are completed!");
            if (((StoreManagementMock) _storeManagementClient).failReason != FailReason.DestToteError)
                Assert.Fail("Wrong fail reason sent to SM");
            var totesWaiting = _totesReadyForPicking.ToList();
            if(!totesWaiting.Any()) Assert.Fail("No totes left under robot");
            if(totesWaiting.Any(tote => tote.Location.zone.function == LocationFunction.Place)) Assert.Fail("Left tote on place position as ready tote");
            Assert.Pass();
        }

        [Test]
        public void TestPickingFromCNV4_2ToCNV1_5DoubleSlot()
        {
            var taskBundle =
                MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV4_2", "RPP1", 1, 2, _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000002", "0000001", "CNV4_2", "RPP1", 2, 4,
                _serviceProvider);

            TestPicking(taskBundle);
        }

        [Test]
        public void TestPickingFromCNV4_2AndNCV3_2ToCNV1_5DoubleSlot()
        {
            var taskBundle =
                MockUtils.MockPickTaskBundle("0000002", "0000001", "CNV4_2", "RPP1", 1, 2, _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000002", "0000001", "CNV4_2", "RPP1", 2, 4,
                _serviceProvider);

            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000002", "0000001", "CNV4_2", "RPP1", 2, 2,
                _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000002", "0000001", "CNV4_2", "RPP1", 3, 2,
                _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000003", "0000001", "CNV3_2", "RPP1", 1, 2,
                _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000003", "0000001", "CNV3_2", "RPP1", 2, 4,
                _serviceProvider);
            MockUtils.AddPickTaskToTaskBundle(taskBundle, "0000003", "0000001", "CNV3_2", "RPP1", 3, 4,
                _serviceProvider);

            TestPicking(taskBundle);
        }

        private void TestPicking(TaskBundle taskBundle)
        {
            var totesToRelease = InitTest(taskBundle, out var partsToPick);

            WaitForPickingCompleteAndTotesReleasedOrErrorToSM(partsToPick, totesToRelease);

            AssertSuccessfulPicking(totesToRelease, partsToPick);
        }

        private void AssertSuccessfulPicking(int totesToRelease, int partsToPick)
        {
            var taskBundleToCheck = _taskBundles.GetFirstPickTaskBundle();
            if (taskBundleToCheck != null && taskBundleToCheck.tasks
                .Any(task => task.taskStatus != RcsTaskStatus.Complete))
                Assert.Fail("Not all tasks from task bundle are completed!");
            if (_totesReleased != totesToRelease)
                Assert.Fail($"Not all totes ware released!, Totes released {_totesReleased}");
            if (((StoreManagementMock) _storeManagementClient).partsPicked != partsToPick)
                Assert.Fail("Not all parts ware picked");
            Assert.Pass();
        }

        private int InitTest(TaskBundle taskBundle, out int partsToPick)
        {
            _taskBundles.ToList().ForEach(tb => _taskBundles.Remove(tb));
            var dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();

            taskBundle.tasks.OfType<PickTask>().GroupBy(task => task.sourceTote.toteId).ToList()
                .ForEach(task =>
                    MockUtils.AddToteToDB(task.First().sourceTote.toteId, dbContext.toteTypes.First(), "A", dbContext));
            taskBundle.tasks.OfType<PickTask>().GroupBy(task => task.destTote.toteId).ToList()
                .ForEach(task =>
                    MockUtils.AddToteToDB(task.First().destTote.toteId, dbContext.toteTypes.First(), "B", dbContext));

            _taskBundles.Add(taskBundle);

            var totesToRelease = _taskBundles.GetFirstPickTaskBundle().tasks.OfType<PickTask>()
                .Select(task => task.sourceTote.toteId).Distinct().Count();

            partsToPick = _taskBundles.GetFirstPickTaskBundle().tasks.OfType<PickTask>()
                .Select(task => task.quantity).Sum();

            taskBundle.tasks.OfType<PickTask>().GroupBy(task => task.destTote.toteId).ToList()
                .ForEach(task =>
                {
                    var tote = dbContext.totes.First(tote => tote.toteBarcode == task.First().destTote.toteId);
                    PlcService.MockScanNotification(task.First().destTote.pickLocation.plcId,
                        tote.toteBarcode, tote.type);
                });

            taskBundle.tasks.OfType<PickTask>().GroupBy(task => task.sourceTote.toteId).ToList()
                .ForEach(task =>
                {
                    var tote = dbContext.totes.First(tote => tote.toteBarcode == task.First().sourceTote.toteId);
                    PlcService.MockScanNotification(task.First().sourceTote.pickLocation.plcId,
                        tote.toteBarcode, tote.type);
                });
            return totesToRelease;
        }


        private void WaitForPickingCompleteAndTotesReleasedOrErrorToSM(int partsToPick, int totesToRelease)
        {
            var failTime = DateTime.Now.AddSeconds(30);
            while ((((StoreManagementMock) _storeManagementClient).partsPicked != partsToPick ||
                    totesToRelease != _totesReleased) && ((StoreManagementMock)_storeManagementClient).failReason == null && DateTime.Now < failTime)
            {
                Thread.Sleep(10);
            }

            _logger.LogInformation($"Totes released when finished waiting {_totesReleased}");
        }

        public override void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            
        }

        public override void ToteReady(PrepareForPicking prepareForPicking)
        {

        }

        public override void ReturnTote(string toteBarcode)
        {
            _totesReleased += 1;
            _logger.LogInformation($"Tote was released. Total released: {_totesReleased}");
        }
    }
}