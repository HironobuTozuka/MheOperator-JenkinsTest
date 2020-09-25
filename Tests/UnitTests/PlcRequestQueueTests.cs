using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Plc;
using Common.Models.Tote;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using PlcRequestQueueService;

namespace Tests
{
    class PlcRequestQueueTests
    {
        protected ILoggerFactory _loggerFactory;
        protected IServiceProvider _serviceProvider;
        protected ILogger<PlcRequestQueueTests> _logger;
        protected IConfiguration _configuration;
        private ICompositeService _kafkaServices;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var useDockerCompose = (string) _configuration.GetSection("Tests").GetSection("DockerCompose")
                .GetValue(typeof(string), "Required");
            if ("true".Equals(useDockerCompose))
            {
                //Create containers for kafka
                var dockerFile = Path.Combine(Directory.GetCurrentDirectory(),
                    "IntegrationTestBase/docker-compose.yml");

                _kafkaServices =
                    new Builder().UseContainer()
                        .UseCompose()
                        .FromFile(dockerFile)
                        .RemoveOrphans()
                        .Build()
                        .Start();
            }
            //Thread.Sleep(1000);

            _serviceProvider = new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("Default", LogLevel.Debug);
                })
                .AddMemoryCache()
                .BuildServiceProvider();

            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();

            _logger = _loggerFactory.CreateLogger<PlcRequestQueueTests>();
        }

        // [TearDown]
        // public void TearDown()
        // {
        //     _kafkaServices.Stop();
        // }

        [Test]
        public async Task TestScanNotificationQueue()
        {
            var sanNotificationDispatcher = new ScanNotificationDispatcherMock();

            var kafkaConsumerGroup = new KafkaConsumerGroup(_loggerFactory, _configuration, "TestQueue");
            kafkaConsumerGroup.Subscribe(new ScanNotificationConsumer(_loggerFactory,
                sanNotificationDispatcher));

            var scanNotificationProducer = new ScanNotificationProducer(_loggerFactory, _configuration);

            await scanNotificationProducer.Produce(MockScanNotification("00000001"));
            await scanNotificationProducer.Produce(MockScanNotification("00000002"));
            await scanNotificationProducer.Produce(MockScanNotification("00000003"));
            await scanNotificationProducer.Produce(MockScanNotification("00000004"));

            List<string> barcodes = new List<string>();
            barcodes.Add("00000001");
            barcodes.Add("00000002");
            barcodes.Add("00000003");
            barcodes.Add("00000004");

            WaitForScanNotifications(barcodes, sanNotificationDispatcher);

            var receivedScans = sanNotificationDispatcher.recievedScanNotifications.ToList();

            if (receivedScans.Any(sn => sn.ToteBarcode == "00000001") &&
                receivedScans.Any(sn => sn.ToteBarcode == "00000002") &&
                receivedScans.Any(sn => sn.ToteBarcode == "00000003") &&
                receivedScans.Any(sn => sn.ToteBarcode == "00000004"))
                Assert.Pass();
            Assert.Fail();
        }

        private ScanNotificationModel MockScanNotification(string toteBarcode)
        {
            return new ScanNotificationModel()
            {
                LocationId = "CNV1_1",
                ToteRotation = ToteRotation.normal,
                ToteType = new RequestToteType(ToteHeight.high, TotePartitioning.bipartite),
                ToteBarcode = toteBarcode
            };
        }

        private void WaitForScanNotifications(List<string> barcodes,
            ScanNotificationDispatcherMock sanNotificationDispatcher)
        {
            DateTime maxTime = DateTime.Now.AddSeconds(30);
            while (DateTime.Now < maxTime && barcodes.Any(barcode =>
                sanNotificationDispatcher.recievedScanNotifications.All(sn => sn.ToteBarcode != barcode)))
            {
                Thread.Sleep(10);
            }
        }

        public class ScanNotificationDispatcherMock : IScanNotificationListenerRegistry
        {
            public List<ScanNotificationModel> recievedScanNotifications = new List<ScanNotificationModel>();

            public async Task NotifyListeners(ScanNotificationModel scanNotification)
            {
                await Task.Run(() =>
                {
                    recievedScanNotifications.Add(scanNotification);
                });
            }
        }
    }
}