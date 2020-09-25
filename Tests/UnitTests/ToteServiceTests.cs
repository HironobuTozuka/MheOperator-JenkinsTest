using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Tote;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RcsLogic.Services;

namespace Tests
{
    public class ToteServiceTests
    {
        private LocationRepository _locationRepository;
        private ToteService _toteService;
        private ToteRepository _toteRepository;


        [Test]
        public void NoReadToteCreationTest()
        {
            var location = _locationRepository.GetLocationByFunction(LocationFunction.Technical);
            var tote1 = _toteService.CreateNoReadTote(location, location);

            if (tote1 == null || tote1.storageLocation?.zone?.function != LocationFunction.Technical ||
                tote1.status != ToteStatus.NoRead || !tote1.toteBarcode.Equals("NOREAD1")) Assert.Fail();
            var tote2 = _toteService.CreateNoReadTote(location, location);
            if (tote2 == null || tote2.storageLocation?.zone?.function != LocationFunction.Technical ||
                tote2.status != ToteStatus.NoRead || !tote2.toteBarcode.Equals("NOREAD2")) Assert.Fail();
            Assert.Pass();
        }
        
        [Test]
        public void SaveToteTest()
        {
            var scanNotification = new ScanNotificationModel()
            {
                LocationId = "CNV1_3",
                ToteBarcode = "0000001",
                ToteRotation = ToteRotation.normal,
                ToteType = new RequestToteType()
                {
                    ToteHeight = ToteHeight.high,
                    TotePartitioning = TotePartitioning.bipartite
                }
            };

            _toteService.SaveTote(scanNotification);
            var tote = _toteRepository.GetToteByBarcode("0000001");
            
            if(tote?.location?.plcId.Equals("CNV1_3") == true 
               && tote?.type?.toteHeight == ToteHeight.high
               && tote?.type?.totePartitioning == TotePartitioning.bipartite) Assert.Pass();
            Assert.Fail();
            
        }

        [SetUp]
        public void Setup()
        {
            var serviceProvider = CreateServiceProvider();
            _toteRepository = new ToteRepository(serviceProvider);
            _locationRepository = new LocationRepository(serviceProvider);
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            _toteService = new ToteService(loggerFactory, _toteRepository, _locationRepository, new StoreManagementMock(loggerFactory));
        }

        private static ServiceProvider CreateServiceProvider()
        {
            var serviceProvider = new ServiceCollection().AddDbContext<StoreDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: "StoreDatabase");
                })
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.SetMinimumLevel(LogLevel.Trace)
                        .AddFilter("Default", LogLevel.Trace)
                        .AddFilter("Microsoft", LogLevel.Warning);
                })
                .BuildServiceProvider();
            StoreDbContext _dbContext = serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<StoreDbContext>();
            _dbContext.Database.EnsureCreated();
            return serviceProvider;
        }
    }
}