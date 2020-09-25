using System;
using System.Collections.Generic;
using Common;
using Common.Models.Gate;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RcsLogic;
using RcsLogic.Gates;
using RcsLogic.Models;
using RcsLogic.Models.Device;
using RcsLogic.Services;

namespace Tests
{
    public class GateTests
    {
        TaskId taskId = new TaskId("testTask");
        Tote tote = new Tote(){toteBarcode = "loremipsum"};
        private Mock<IPlcService> plcService;
        private ILoggerFactory loggerFactory;
        private Mock<IConfiguration> config;
        private Mock<IStoreManagementClient> smCli;
        private Mock<LocationService> locService;
        private Mock<IMujinClient> mujinClient;
        private Mock<ToteRepository> toteRepo;
        private Mock<LocationRepository> loRepo;
        private Mock<ServicedLocationProvider> servicedLocationProvider;
        private Mock<IReturnToteHandler> returnToteHandler;
        private TaskBundleService tbs;
        private Mock<IDeliveryCompleteDevice> deliveryCompleteDev;
        private Mock<RoutingService> routingService;
        
        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection().AddDbContext<StoreDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: "StoreDatabase");
                })
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("Default", LogLevel.Information);
                })
                .AddMemoryCache()
                .BuildServiceProvider();
            plcService = new Mock<IPlcService>();
            loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            config = new Mock<IConfiguration>();
            smCli = new Mock<IStoreManagementClient>();
            mujinClient = new Mock<IMujinClient>();
            toteRepo = new Mock<ToteRepository>(serviceProvider);
            loRepo = new Mock<LocationRepository>(serviceProvider);
            returnToteHandler = new Mock<IReturnToteHandler>();
            deliveryCompleteDev = new Mock<IDeliveryCompleteDevice>();
            var deviceStatusService = new DeviceStatusService(config.Object);
            routingService = new Mock<RoutingService>(new Mock<IMemoryCache>().Object, serviceProvider, loRepo.Object, deviceStatusService);
            locService = new Mock<LocationService>(loggerFactory, loRepo.Object, null, routingService.Object);
            servicedLocationProvider = new Mock<ServicedLocationProvider>(serviceProvider, loggerFactory);
            tbs = new TaskBundleService(loggerFactory, smCli.Object, locService.Object, mujinClient.Object,
                toteRepo.Object, loRepo.Object);

            
        }
        
        [Test]
        public void TestDeliveryCompleteTrigger()
        {
            deliveryCompleteDev.Setup(device => device.CompleteDelivery(It.IsNotNull<Tote>())).Callback<Tote>(it =>
                {
                    if(it.Equals(tote)) Assert.Pass();
                    Assert.Fail();
                });
            _= new DeliveryCompleteTrigger(tote, 0.5f, deliveryCompleteDev.Object);
        }
        
        [Test]
        public void TestOrderGateCompleteDelivery()
        {

            returnToteHandler.Setup(it => it.ReturnTote(It.IsNotNull<string>())).Callback<string>(bar =>
            {
                if (bar.Equals(tote.toteBarcode) 
                    && tbs.GetTaskBundle(taskId)==null) Assert.Pass();
                    Assert.Fail();
            });

            tbs.Add(new TaskBundle()
            {
                tasks = new List<TaskBase>()
                {
                    new DeliverTask()
                    {
                        isInternal = false,
                        lastUpdateDate = DateTime.Now,
                        processingStartedDate = DateTime.Now,
                        slots = new []{0},
                        taskId = taskId,
                        taskStatus =  RcsTaskStatus.Executing,
                        toteId = tote.toteBarcode
                    }
                }
            });
            
            if (tbs.GetTaskBundle(taskId)==null) Assert.Fail();
            var orderGate = new OrderGates(new DeviceId("orderGate"), plcService.Object, loggerFactory, tbs, 
                servicedLocationProvider.Object, config.Object);
            
            orderGate.RegisterReturnHandler(returnToteHandler.Object);
            orderGate.CompleteDelivery(tote);
        }
        
        [Test]
        public void TestOrderGateExpose()
        {
            var gateId = "Gate1";
            bool gateOpened = false;
            var deliverTask = new DeliverTask()
            {
                isInternal = false,
                lastUpdateDate = DateTime.Now,
                processingStartedDate = DateTime.Now,
                slots = new[] {0},
                taskId = taskId,
                taskStatus = RcsTaskStatus.Executing,
                toteId = tote.toteBarcode
            };
            returnToteHandler.Setup(it => it.ReturnTote(It.IsNotNull<string>())).Callback<string>(bar =>
            {
                if (bar.Equals(tote.toteBarcode) 
                    && tbs.GetTaskBundle(taskId)==null
                    && gateOpened) Assert.Pass();
                Assert.Fail();
            });

            tbs.Add(new TaskBundle()
            {
                tasks = new List<TaskBase>() { deliverTask }
            });
            
            plcService.Setup(it => it.OpenGate(It.IsNotNull<GateDescription>())).Callback<GateDescription>(it =>
            {
                if(it.gateId.Id.Equals(gateId)) gateOpened = true;
            });
            
            if (tbs.GetTaskBundle(taskId)==null) Assert.Fail();
            var orderGate = new OrderGates(new DeviceId("orderGate"), plcService.Object, loggerFactory, tbs, 
                servicedLocationProvider.Object, config.Object);
            
            orderGate.RegisterReturnHandler(returnToteHandler.Object);
            orderGate.Expose(new SlotsToExpose(deliverTask, new Location(){zone = new Zone() {plcGateId = gateId}}, ToteRotation.normal, tote));
        }

        [Test]
        public void TestLoadingGateExpose()
        {
            var gateId = "Gate1";
            bool gateOpened = false;
            var deliverTask = new DeliverTask()
            {
                isInternal = false,
                lastUpdateDate = DateTime.Now,
                processingStartedDate = DateTime.Now,
                slots = null,
                taskId = taskId,
                taskStatus = RcsTaskStatus.Executing,
                toteId = tote.toteBarcode
            };
            
            tbs.Add(new TaskBundle()
            {
                taskBundleId = new TaskBundleId("LoremIpsum"),
                tasks = new List<TaskBase>() { deliverTask }
            });
            if (tbs.GetTaskBundle(taskId)==null) Assert.Fail();

            plcService.Setup(it => it.OpenGate(It.IsNotNull<GateDescription>())).Callback<GateDescription>(it =>
            {
                if(it.gateId.Id.Equals(gateId)) gateOpened = true;
            });

            var loadingGate = new LoadingGate(new DeviceId("GATE_LOAD"), plcService.Object, loggerFactory, tbs,
                servicedLocationProvider.Object, toteRepo.Object, routingService.Object);
            
            loadingGate.Expose(new SlotsToExpose(deliverTask, new Location(){zone = new Zone() {plcGateId = gateId}}, ToteRotation.normal, tote));
            
            MockUtils.WaitFor(() => tbs.GetTaskBundle(taskId)==null);
            if (tbs.GetTaskBundle(taskId)==null && gateOpened) Assert.Pass();
            Assert.Fail();
        }
    }
}