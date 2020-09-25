using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RcsLogic.Watchdog
{
    public class WatchdogExecutor : IDisposable
    {
        private List<IWatchdog> _watchdogs = new List<IWatchdog>();
        private ILogger<WatchdogExecutor> _logger;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _task;

        public WatchdogExecutor(ILogger<WatchdogExecutor> logger)
        {
            _logger = logger;
            _task = Task.Run(Run, _cancellationTokenSource.Token);
        }

        public void RegisterWatchdog(IWatchdog watchdog)
        {
            _watchdogs.Add(watchdog);
            _logger.LogInformation("Registered watchdog: {0}", watchdog);
        }
        
        private async void Run()
        {
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _watchdogs.ToList().ForEach(watchdog => watchdog.Execute());
                    await Task.Delay(30000, _cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"WatchdogExecutor Cancelled");
            }
        }
        
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _task?.Wait();
            _cancellationTokenSource?.Dispose();
        }
    }
}