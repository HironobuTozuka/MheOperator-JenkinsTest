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
using Common.Models;
using Common.Models.Location;
using Common.Models.Task;
using RcsLogic.Models;

namespace Tests
{
    public class ConveyingTests : DeviceTests
    {
        public new ILogger<ConveyingTests> _logger;

        bool robotPickRecieved = false;
        bool robotPlaceRecieved = false;


        [SetUp]
        public void Setup()
        {
            base.Setup(true, false, true, false, false, false);
            _logger = _loggerFactory.CreateLogger<ConveyingTests>();

            robotPickRecieved = false;
            robotPlaceRecieved = false;
            _logger.LogInformation("Test preparation complete");
        }


        [Test]
        public void TestTransferFromCNV1_3ToCNV3_2DestToteOnRackB()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "B", _dbContext);
            var toteType = _dbContext.toteTypes.First();
            _dbContext.Dispose();

            _taskBundles.Add(MockUtils.MockPickTaskBundle("0000001", "0000002", "CNV3_2", "RPP1", 1, 1,
                _serviceProvider));

            PlcService.MockScanNotification("CNV1_3", "0000001", toteType);
            PlcService.MockScanNotification("RPP1", "0000002", toteType);

            WaitForTransfersComplete();

            _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            var destTote = _dbContext.totes.Include(tote => tote.location).First(tote => tote.toteBarcode == "0000002");
            var sourceTote = _dbContext.totes.Include(tote => tote.location)
                .First(tote => tote.toteBarcode == "0000001");

            if (!robotPickRecieved) Assert.Fail("No robot ready for pick event recieved!");
            if (!robotPlaceRecieved) Assert.Fail("No robot ready for place event recieved!");
            if (destTote.location.plcId != "RPP1")
                Assert.Fail("Dest tote location in DB is different than place location");
            if (sourceTote.location.plcId != "CNV3_2" && sourceTote.location.plcId != "CNV4_2")
                Assert.Fail("Source tote location in DB is different than pick location");
            Assert.Pass();
        }

        [Test]
        public void TestTransferFromCNV1_3ToCNV4_2DestToteOnRackB()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "B", _dbContext);
            var toteType = _dbContext.toteTypes.First();
            _taskBundles.Add(MockUtils.MockPickTaskBundle("0000001", "0000002", "CNV4_2", "RPP1", 1, 1,
                _serviceProvider));

            PlcService.MockScanNotification("CNV1_3", "0000001", toteType);
            PlcService.MockScanNotification("RPP1", "0000002", toteType);


            WaitForTransfersComplete();

            _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            var destTote = _dbContext.totes.Include(tote => tote.location).First(tote => tote.toteBarcode == "0000002");
            var sourceTote = _dbContext.totes.Include(tote => tote.location)
                .First(tote => tote.toteBarcode == "0000001");

            if (!robotPickRecieved) Assert.Fail("No robot ready for pick event recieved!");
            if (!robotPlaceRecieved) Assert.Fail("No robot ready for place event recieved!");
            if (destTote.location.plcId != "RPP1")
                Assert.Fail("Dest tote location in DB is different than place location");
            if (sourceTote.location.plcId != "CNV4_2" && sourceTote.location.plcId != "CNV3_2")
                Assert.Fail("Source tote location in DB is different than pick location");
            Assert.Pass();
        }

        [Test]
        public void TestTransferFromCNV1_3ToCNV4_2DestToteOnRackA()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("0000001", _dbContext.toteTypes.First(), "A", _dbContext);
            MockUtils.AddToteToDB("0000002", _dbContext.toteTypes.First(), "A", _dbContext);
            var toteType = _dbContext.toteTypes.First();
            _dbContext.Dispose();

            _taskBundles.Add(MockUtils.MockPickTaskBundle("0000001", "0000002", "CNV4_2", "RPP1", 1, 1,
                _serviceProvider));

            PlcService.MockScanNotification("CNV1_3", "0000001", toteType);
            Thread.Sleep(2000);
            PlcService.MockScanNotification("CNV1_3", "0000002", toteType);


            WaitForTransfersComplete();

            _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            var destTote = _dbContext.totes.Include(tote => tote.location).First(tote => tote.toteBarcode == "0000002");
            var sourceTote = _dbContext.totes.Include(tote => tote.location)
                .First(tote => tote.toteBarcode == "0000001");

            if (!robotPickRecieved) Assert.Fail("No robot ready for pick event recieved!");
            if (!robotPlaceRecieved) Assert.Fail("No robot ready for place event recieved!");
            if (destTote.location.plcId != "RPP1")
                Assert.Fail("Dest tote location in DB is different than place location");
            if (sourceTote.location.plcId != "CNV4_2" && sourceTote.location.plcId != "CNV3_2")
                Assert.Fail("Source tote location in DB is different than pick location");
            Assert.Pass();
        }

        private void WaitForTransfersComplete()
        {
            var failTime = DateTime.Now.AddSeconds(30);

            while (!(robotPickRecieved && robotPlaceRecieved) && DateTime.Now < failTime)
            {
                Thread.Sleep(10);
            }

            bool InLocation1 = false;
            bool InLocation2 = false;

            while (!InLocation1 && !InLocation2 && DateTime.Now < failTime)
            {
                var _dbContext = _serviceProvider.CreateScope().ServiceProvider
                    .GetRequiredService<Data.StoreDbContext>();
                var destTote = _dbContext.totes.Include(tote => tote.location)
                    .First(tote => tote.toteBarcode == "0000002");
                var sourceTote = _dbContext.totes.Include(tote => tote.location)
                    .First(tote => tote.toteBarcode == "0000001");
                if (destTote.location != null) InLocation1 = true;
                if (sourceTote.location != null) InLocation2 = true;
                _dbContext.Dispose();
            }

            Thread.Sleep(80);
        }


        public override void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
        }

        public override void ToteReady(PrepareForPicking prepareForPicking)
        {
            TaskBundle taskBundle = _taskBundles.GetFirstPickTaskBundle();

            _logger.LogInformation("Ready for picking sent to robot for {1} on location {2}", prepareForPicking.Tote.toteBarcode,
                prepareForPicking.Location.plcId);
            if (prepareForPicking.Tote.toteBarcode == ((PickTask) taskBundle.tasks.First()).sourceTote.toteId) robotPickRecieved = true;
            if (prepareForPicking.Tote.toteBarcode == ((PickTask) taskBundle.tasks.First()).destTote.toteId) robotPlaceRecieved = true;
        }

        public override void ReturnTote(string toteBarcode)
        {
        }
    }
}