using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace rmdev.ScopedTimerHostedService
{

    public delegate void TimerAction<T>(T service) where T : notnull;
    public delegate void TimerActionWithCancelationToken<T>(T service, CancellationToken cancellationToken) where T : notnull;

    internal sealed class ScopedTimerHostedService<T> : IHostedService where T : notnull
    {
        private readonly TimerAction<T>? _timerAction;
        private readonly TimerActionWithCancelationToken<T>? _timerActionWithCancelationToken;
        private readonly IServiceProvider _serviceProvider;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly SemaphoreSlim _semaphore;
        private CancellationTokenSource? _cancellationTokenSource;

        public ScopedTimerHostedService(IServiceProvider serviceProvider, double interval, int concurrent, TimerAction<T>? timerAction = null, TimerActionWithCancelationToken<T>? timerActionWithCancelationToken = null)
        {
            _timerAction = timerAction;
            _timerActionWithCancelationToken = timerActionWithCancelationToken;
            if(_timerAction == null && _timerActionWithCancelationToken == null)
                throw new ArgumentNullException(nameof(timerAction));
            _serviceProvider = serviceProvider;
            _timer.Interval = interval;
            _timer.Elapsed += ScopeControl;
            _semaphore = new SemaphoreSlim(concurrent);
        }

        private void ScopeControl(object? sender, ElapsedEventArgs e)
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger<ScopedTimerHostedService<T>>>();
            if (_semaphore.Wait(0))
            {
                logger?.LogInformation("Doing...");
                try
                {
                    var service = scope.ServiceProvider.GetRequiredService<T>();
                    _timerAction?.Invoke(service);
                    _timerActionWithCancelationToken?.Invoke(service, _cancellationTokenSource!.Token);
                }
                finally
                {
                    _semaphore.Release();
                    logger?.LogInformation("Done.");
                }
            }
            else
            {
                logger?.LogWarning("It was not done because it reached the concurrency limit.");
            }
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            _timer.Start();
            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            _cancellationTokenSource?.Cancel();
            return Task.CompletedTask;
        }
    }
}
