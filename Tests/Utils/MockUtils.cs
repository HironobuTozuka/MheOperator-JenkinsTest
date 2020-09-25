using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Common.Models;
using Common.Models.Location;
using Common.Models.Plc;
using Common.Models.Task;
using Common.Models.Tote;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    class MockUtils
    {
        public static Common.Models.ScanNotificationModel CreateScanNotification(string locationId, string toteBarcode,
            IServiceProvider _serviceProvider)
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            return new Common.Models.ScanNotificationModel
            {
                LocationId = locationId,
                ToteBarcode = toteBarcode,
                ToteRotation = ToteRotation.normal,
                ToteType = new RequestToteType(_dbContext.toteTypes.First())
            };
        }

        public static void AddToteToDB(string toteBarcode, ToteType type, string rackId,
            Data.StoreDbContext _dbContext, int minCol = 0, Location storageLocation = null)
        {
            int storageLocationId;
            if (storageLocation == null)
            {
                storageLocationId = _dbContext.locations.First(location =>
                    location.rack.Contains(rackId) && location.storedTote == null && location.col > minCol).id;
            }
            else
            {
                storageLocationId = storageLocation.id;
            }

            _dbContext.totes.Add(new Tote()
            {
                toteBarcode = toteBarcode,
                typeId = type.id,
                storageLocationId = storageLocationId,
                locationId = storageLocationId
            });
            _dbContext.SaveChanges();
        }

        public static void AddPickTaskToTaskBundle(TaskBundle taskBundle, string sourceToteBarcode,
            string destToteBarcode, string pickLocationID, string destLocationID, int pickSlot, int partsToPick,
            IServiceProvider _serviceProvider)
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            taskBundle.tasks.Add(new PickTask()
            {
                barcode = "12345678",
                destTote = new PickToteData()
                {
                    slotId = 1,
                    toteId = destToteBarcode,
                    pickLocation = _dbContext.locations.First(location => location.plcId == destLocationID)
                },
                sourceTote = new PickToteData()
                {
                    slotId = pickSlot,
                    toteId = sourceToteBarcode,
                    pickLocation = _dbContext.locations.First(location => location.plcId == pickLocationID)
                },
                quantity = partsToPick,
                taskId = new TaskId(Guid.NewGuid().ToString()),
                taskStatus = RcsTaskStatus.Idle
            });
        }

        public static TaskBundle MockPickTaskBundle(string sourceToteBarcode, string destToteBarcode,
            string pickLocationID, string destLocationID, int pickSlot, int partsToPick,
            IServiceProvider _serviceProvider)
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            return new TaskBundle()
            {
                taskBundleId = new TaskBundleId("h32ruhewfw97feg"),
                creationDate = DateTime.Now,
                tasks = new List<TaskBase>()
                {
                    new PickTask()
                    {
                        barcode = "12345678",
                        sourceTote = new PickToteData()
                        {
                            slotId = 1,
                            toteId = sourceToteBarcode,
                            pickLocation = _dbContext.locations.First(location => location.plcId == pickLocationID)
                        },
                        destTote = new PickToteData()
                        {
                            slotId = 1,
                            toteId = destToteBarcode,
                            pickLocation = _dbContext.locations.First(location => location.plcId == destLocationID)
                        },
                        quantity = 2,
                        taskId = new TaskId(Guid.NewGuid().ToString()),
                        taskStatus = RcsTaskStatus.Idle,
                        lastUpdateDate = DateTime.Now,
                        processingStartedDate = DateTime.Now
                    }
                }
            };
        }

        public static TaskBundle MockMoveTaskBundle(string toteBarcode, string destLocationID,
            IServiceProvider _serviceProvider)
        {
            var _dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<Data.StoreDbContext>();
            var destLocation = _dbContext.locations.Include(loc => loc.zone)
                .First(location => location.plcId == destLocationID);
            return new TaskBundle()
            {
                taskBundleId = new TaskBundleId("h32ruhewfw97feg"),
                creationDate = DateTime.Now,
                tasks = new List<TaskBase>()
                {
                    new MoveTask()
                    {
                        taskId = new TaskId(Guid.NewGuid().ToString()),
                        toteId = toteBarcode,
                        destLocation = destLocation,
                        taskStatus = RcsTaskStatus.Idle,
                        destZone = destLocation?.zone?.zoneId ?? new ZoneId(_dbContext.zones.First().id),
                        lastUpdateDate = DateTime.Now,
                        processingStartedDate = DateTime.Now
                    }
                }
            };
        }
        public static void WaitFor(Func<Boolean> condition)
        {
            var failTime = DateTime.Now.AddSeconds(30);
            while ((condition() == false) && DateTime.Now < failTime)
            {
                
                Thread.Sleep(1);
            }
            Thread.Sleep(50);
        }
    }
}