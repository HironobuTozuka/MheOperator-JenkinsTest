using System.IO;
using System.Linq;
using Common;
using Common.Models;
using Common.Models.Plc;
using Common.Models.Tote;
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
    public class ToteLocationUpdatingScanNotificationListenerTests
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
                .AddSingleton<ToteService>()
                .AddSingleton<UnknownToteRouter>()
                .AddSingleton<IStoreManagementClient, StoreManagementMock>()
                .AddSingleton<ToteLocationUpdatingScanNotificationListener>()
                .AddMemoryCache()
                .BuildServiceProvider();
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            _dbContext.Database.EnsureCreated();
        }

        [Test]
        public void ToteWithUnknownLocationReceivedScan()
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("00000001", _dbContext.toteTypes.First(), "A", _dbContext);
            var tote = _serviceProvider.GetRequiredService<ToteRepository>().GetToteByBarcode("00000001");
            var location = _serviceProvider.GetRequiredService<LocationRepository>().GetLocationByPlcId("RPP1");
            var smClient = _serviceProvider.GetRequiredService<IStoreManagementClient>();
            _serviceProvider.GetRequiredService<ToteRepository>().UpdateToteLocation(tote, null);
            _serviceProvider.GetRequiredService<ToteRepository>().UpdateToteStatus(tote, ToteStatus.LocationUnknown);

            var toteLocationUpdatingScanNotificationListener =
                _serviceProvider.GetRequiredService<ToteLocationUpdatingScanNotificationListener>();
            toteLocationUpdatingScanNotificationListener.ProcessScanNotification(new ScanNotificationModel()
            { 
                LocationId = location.plcId,
                ToteBarcode = tote.toteBarcode,
                ToteRotation = ToteRotation.normal,
                ToteType = new RequestToteType()
                {
                    ToteHeight = ToteHeight.high,
                    TotePartitioning = TotePartitioning.bipartite
                }
            });
            
            tote = _serviceProvider.GetRequiredService<ToteRepository>().GetToteByBarcode("00000001");
            
            Assert.That(
                tote.status == ToteStatus.Ready &&
                tote.location.Equals(location) &&
                ((StoreManagementMock) smClient).SentNotifications.Any(notification => notification.tote.toteBarcode == tote.toteBarcode && notification.toteStatus == ToteStatus.Ready));
        }
    }
}