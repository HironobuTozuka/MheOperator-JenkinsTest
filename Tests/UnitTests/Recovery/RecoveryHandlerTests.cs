using System;
using System.IO;
using Common;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RcsLogic.RcsController;
using RcsLogic.RcsController.Recovery;
using RcsLogic.Services;

namespace Tests
{
    public class RecoveryHandlerTests
    {
        private IServiceProvider _serviceProvider;


        [SetUp]
        public void SetUp()
        {
            _serviceProvider = new ServiceCollection().AddDbContext<StoreDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName: "StoreDatabase");
                })
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("Default", LogLevel.Trace);
                })
                .AddSingleton((IConfiguration)new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build())
                .AddSingleton<LocationRepository>()
                .AddSingleton<ToteRepository>()
                .AddSingleton<LocationService>()
                .AddSingleton<TaskBundleService>()
                .AddSingleton<UnknownToteRouter>()
                .AddSingleton<IStoreManagementClient, StoreManagementMock>()
                .AddSingleton<IMujinClient, MujinClientMock>()
                .AddSingleton<RoutingService>()
                .AddSingleton<DeviceStatusService>()
                .AddSingleton<RecoveryHandler>()
                .AddMemoryCache()
                .BuildServiceProvider();
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            _dbContext.Database.EnsureCreated();
        }
        
        [Test]
        public void NoToteOnSourceLocationRecovery()
        {
            var handler = _serviceProvider.GetRequiredService<RecoveryHandler>();
            var strategy1 = handler.Strategy(SystemSortCodes.Error_11_NoToteOnSourceOnConveying);
            var strategy2 = handler.Strategy(SystemSortCodes.PickError_CA_P_1210_SourceLocationEmpty);
            var strategy3 = handler.Strategy(SystemSortCodes.PickError_CA_P_1211_SourceLocationEmpty);
            var strategy4 = handler.Strategy(SystemSortCodes.PickError_CB_P_1210_SourceLocationEmpty);
            var strategy5 = handler.Strategy(SystemSortCodes.PickError_CB_P_1211_SourceLocationEmpty);
            if(strategy1.GetType() == strategy2.GetType() &&  
               strategy2.GetType() == strategy3.GetType() && 
               strategy3.GetType() == strategy4.GetType() && 
               strategy4.GetType() == strategy5.GetType() && 
               strategy1.GetType() == typeof(NoToteOnSourceRecovery)) Assert.Pass();
            Assert.Fail();
        }
        
    }
}