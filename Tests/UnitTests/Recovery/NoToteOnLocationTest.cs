using System;
using System.IO;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Location;
using Common.Models.Tote;
using Common.Models.Transfer;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RcsLogic;
using RcsLogic.Models;
using RcsLogic.RcsController;
using RcsLogic.RcsController.Recovery;
using RcsLogic.Services;

namespace Tests
{
    public class NoToteOnLocationTest
    {
        private ServiceProvider _serviceProvider;

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
                .AddSingleton<NoToteOnSourceRecovery>()
                .AddMemoryCache()
                .BuildServiceProvider();
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            _dbContext.Database.EnsureCreated();
        }
        
        [Test]
        public void RecoverNoTote()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("00000001", _dbContext.toteTypes.First(), "A", _dbContext);
            var tote = _serviceProvider.GetRequiredService<ToteRepository>().GetToteByBarcode("00000001");
            
            var source = _serviceProvider.GetRequiredService<LocationRepository>().GetLocationByPlcId("RPP1");
            _serviceProvider.GetRequiredService<ToteRepository>().UpdateToteLocation(tote, source);
            var dest = _serviceProvider.GetRequiredService<LocationRepository>()
                .GetLocationByFunction(LocationFunction.Staging);

            var recovery = _serviceProvider.GetRequiredService<NoToteOnSourceRecovery>();
            var smClient = _serviceProvider.GetRequiredService<IStoreManagementClient>();
            var transferMock = new Mock<ITransferCompletingDevice>();
            var recoverTransfers = recovery.Recover(transferMock.Object, new TransferResult()
            {
                RequestedTransfer = new Transfer()
                {
                    tote = tote,
                    destLocation = dest,
                    sourceLocation = source,
                    task = _serviceProvider.GetRequiredService<TaskBundleService>().GetInternalMoveTask(tote, dest)
                },
                RequestDone = new ToteTransferRequestDoneModel()
                {
                    actualDestLocationId = source.plcId,
                    requestedDestLocationId = dest.plcId,
                    requestId = new TransferId(Guid.NewGuid().ToString()),
                    sortCode = SystemSortCodes.PickError_CA_P_1210_SourceLocationEmpty.Code,
                    sourceLocationId = source.plcId,
                    sourceToteBarcode = tote.toteBarcode
                },
                SystemSortCode = SystemSortCodes.PickError_CA_P_1210_SourceLocationEmpty
            });
            
            tote = _serviceProvider.GetRequiredService<ToteRepository>().GetToteByBarcode("00000001");
            
            Assert.That(recoverTransfers.Count == 0 && 
                        tote.status == ToteStatus.LocationUnknown &&
                        ((StoreManagementMock) smClient).SentNotifications.Any(notification => notification.tote.toteBarcode == tote.toteBarcode && notification.toteStatus == ToteStatus.LocationUnknown));
            
        }
    }
}