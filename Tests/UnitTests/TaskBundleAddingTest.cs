using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using System.Linq;
using System.Threading;
using Common.Models.Location;
using Common.Models.Task;
using Common.Models.Tote;
using MheOperator.StoreManagementApi.Controllers;
using MheOperator.StoreManagementApi.Models;
using MheOperator.StoreManagementApi.Models.TaskController;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RcsLogic.Models;
using TaskBundle = Common.Models.Task.TaskBundle;

namespace Tests
{
    public class TaskBundleAddingTest : DeviceTests, ITaskBundleAddedListener
    {
        private new ILogger<TaskBundleAddingTest> _logger;
        private bool taskBundleAdded;

        [SetUp]
        public void Setup()
        {
            base.Setup(false, false, false, false, false, false);

            _logger = _loggerFactory.CreateLogger<TaskBundleAddingTest>();
            _taskBundles.RegisterTaskBundleAddedListener(this);
        }

        [Test]
        public void TestAddingMoveTask()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("00000001", _dbContext.toteTypes.First(type => type.toteHeight == ToteHeight.low),
                "B", _dbContext);
            _dbContext.Dispose();

            var taskController = _serviceProvider.GetRequiredService<TaskBundleController>();
            taskController.PostTasks(new MheOperator.StoreManagementApi.Models.TaskController.TaskBundleDataModel()
            {
                taskBundleId = Guid.NewGuid().ToString(), tasks = new List<TaskBaseDataModel>()
                {
                    new MoveTaskDataModel()
                    {
                        taskId = Guid.NewGuid().ToString(),
                        destLocation = new ZoneId("STAGING"),
                        toteId = "00000001"
                    }
                }
            });
            WaitForTaskBundleAdded();
            if (taskBundleAdded) Assert.Pass();
            Assert.Fail("Task bundle adding not successful");
        }

        [Test]
        public void TestAddingInvalidMoveTask_ToteTooHighForStaging()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("00000001", _dbContext.toteTypes.First(type => type.toteHeight == ToteHeight.high),
                "B", _dbContext);
            _dbContext.Dispose();

            var taskController = _serviceProvider.GetRequiredService<TaskBundleController>();
            taskController.PostTasks(new MheOperator.StoreManagementApi.Models.TaskController.TaskBundleDataModel()
            {
                taskBundleId = Guid.NewGuid().ToString(), tasks = new List<TaskBaseDataModel>()
                {
                    new MoveTaskDataModel()
                    {
                        taskId = Guid.NewGuid().ToString(),
                        destLocation = new ZoneId("STAGING"),
                        toteId = "00000001"
                    }
                }
            });
            WaitForTaskBundleAdded();
            if (taskBundleAdded) Assert.Fail("Tote too high, but task was added");
            Assert.Pass();
        }

        [Test]
        public void TestAddingInvalidMoveTask_ToteDoesNotExist()
        {
            var taskController = _serviceProvider.GetRequiredService<TaskBundleController>();
            taskController.PostTasks(new MheOperator.StoreManagementApi.Models.TaskController.TaskBundleDataModel()
            {
                taskBundleId = Guid.NewGuid().ToString(), tasks = new List<TaskBaseDataModel>()
                {
                    new MoveTaskDataModel()
                    {
                        taskId = Guid.NewGuid().ToString(),
                        destLocation = new ZoneId("STAGING"),
                        toteId = "00000001"
                    }
                }
            });
            WaitForTaskBundleAdded();
            if (taskBundleAdded) Assert.Fail("Tote does not exist, but task was added");
            Assert.Pass();
        }

        [Test]
        public void TestAddingInvalidMoveTask_UnexistingZone()
        {
            Data.StoreDbContext _dbContext = _serviceProvider.CreateScope().ServiceProvider
                .GetRequiredService<Data.StoreDbContext>();
            MockUtils.AddToteToDB("00000001", _dbContext.toteTypes.First(type => type.toteHeight == ToteHeight.high),
                "B", _dbContext);
            _dbContext.Dispose();

            var taskController = _serviceProvider.GetRequiredService<TaskBundleController>();
            taskController.PostTasks(new MheOperator.StoreManagementApi.Models.TaskController.TaskBundleDataModel()
            {
                taskBundleId = Guid.NewGuid().ToString(), tasks = new List<TaskBaseDataModel>()
                {
                    new MoveTaskDataModel()
                    {
                        taskId = Guid.NewGuid().ToString(),
                        destLocation = new ZoneId("LOREM_IPSUM"),
                        toteId = "00000001"
                    }
                }
            });
            WaitForTaskBundleAdded();
            if (taskBundleAdded) Assert.Fail("Zone does not exist, but task was added");
            Assert.Pass();
        }

        public override void ProcessScanNotification(ScanNotificationModel scanNotification)
        {
            throw new System.NotImplementedException();
        }

        public override void ToteReady(PrepareForPicking prepareForPicking)
        {
            throw new System.NotImplementedException();
        }

        public override void ReturnTote(string toteBarcode)
        {
            throw new System.NotImplementedException();
        }

        public void HandleNewTaskBundle(TaskBundle newTaskBundle)
        {
            taskBundleAdded = true;
        }

        public void WaitForTaskBundleAdded()
        {
            var failTime = DateTime.Now.AddSeconds(2);
            while (!taskBundleAdded && DateTime.Now < failTime)
            {
                Thread.Sleep(10);
            }
        }
    }
}