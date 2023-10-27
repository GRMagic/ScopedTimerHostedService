using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace rmdev.ScopedTimerHostedService
{
    /// <summary>
    /// Delegate without CancellationToken
    /// </summary>
    /// <typeparam name="T">Service Type</typeparam>
    /// <param name="service">Injected service</param>
    public delegate void TimerAction<in T>(T service) where T : notnull;

    /// <summary>
    /// Delegate without CancellationToken
    /// </summary>
    /// <typeparam name="T">Service Type</typeparam>
    /// <param name="service">Injected service</param>
    public delegate Task TimerActionAsync<in T>(T service) where T : notnull;

    /// <summary>
    /// Delegate with CancellationToken
    /// </summary>
    /// <typeparam name="T">Service Type</typeparam>
    /// <param name="service">Injected service</param>
    /// <param name="cancellationToken">Cancellation is requested when the service is stopping</param>
    public delegate void TimerActionWithCancelationToken<in T>(T service, CancellationToken cancellationToken) where T : notnull;

    /// <summary>
    /// Delegate with CancellationToken
    /// </summary>
    /// <typeparam name="T">Service Type</typeparam>
    /// <param name="service">Injected service</param>
    /// <param name="cancellationToken">Cancellation is requested when the service is stopping</param>
    public delegate Task TimerActionWithCancelationTokenAsync<in T>(T service, CancellationToken cancellationToken) where T : notnull;

    internal sealed class ScopedTimerHostedService<T> : IHostedService where T : notnull
    {
        private readonly TimerAction<T>? _timerAction;
        private readonly TimerActionAsync<T>? _timerActionAsync;
        private readonly TimerActionWithCancelationToken<T>? _timerActionWithCancelationToken;
        private readonly TimerActionWithCancelationTokenAsync<T>? _timerActionWithCancelationTokenAsync;
        private readonly IServiceProvider _serviceProvider;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly SemaphoreSlim _semaphore;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly bool _intervalFromService;

        private ScopedTimerHostedService(IServiceProvider serviceProvider, double interval, int concurrent)
        {
            _intervalFromService = interval <= 0;
            if (_intervalFromService)
            {
                using var scope = serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<T>() as IIntervaledTimer;
                interval = service?.Interval??0;
            }

            _serviceProvider = serviceProvider;
            _timer.Interval = interval;
            _timer.Elapsed += ScopeControl;
            _semaphore = new SemaphoreSlim(concurrent);
        }


        public ScopedTimerHostedService(IServiceProvider serviceProvider, double interval, int concurrent, TimerAction<T> timerAction) : this(serviceProvider, interval, concurrent)
        {
            if (timerAction == null) throw new ArgumentNullException(nameof(timerAction));
            _timerAction = timerAction;
        }

        public ScopedTimerHostedService(IServiceProvider serviceProvider, double interval, int concurrent, TimerActionAsync<T>? timerActionAsync) : this(serviceProvider, interval, concurrent)
        {
            if (timerActionAsync == null) throw new ArgumentNullException(nameof(timerActionAsync));
            _timerActionAsync = timerActionAsync;
        }

        public ScopedTimerHostedService(IServiceProvider serviceProvider, double interval, int concurrent, TimerActionWithCancelationToken<T>? timerActionWithCancelationToken) : this(serviceProvider, interval, concurrent)
        {
            if (timerActionWithCancelationToken == null) throw new ArgumentNullException(nameof(timerActionWithCancelationToken));
            _timerActionWithCancelationToken = timerActionWithCancelationToken;
        }

        public ScopedTimerHostedService(IServiceProvider serviceProvider, double interval, int concurrent, TimerActionWithCancelationTokenAsync<T>? timerActionWithCancelationTokenAsync) : this(serviceProvider, interval, concurrent)
        {
            if (timerActionWithCancelationTokenAsync == null) throw new ArgumentNullException(nameof(timerActionWithCancelationTokenAsync));
            _timerActionWithCancelationTokenAsync = timerActionWithCancelationTokenAsync;
        }

        private void ScopeControl(object? sender, ElapsedEventArgs e)
        {
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetService<ILogger<ScopedTimerHostedService<T>>>();
            if (_semaphore.Wait(0))
            {
                logger?.LogTrace("Doing...");
                try
                {
                    var service = scope.ServiceProvider.GetRequiredService<T>();
                    if (_intervalFromService)
                    {
                        var newInterval = (service as IIntervaledTimer)?.Interval??0;
                        var oldInterval = _timer.Interval;
                        if (oldInterval != newInterval)
                        {
                            _timer.Interval = newInterval;
                            logger?.LogInformation("{TypeOfTimer} was interval changed from {oldInterval}ms to {newInterval}ms.", typeof(T).Name, oldInterval, newInterval);
                        }
                    }
                    _timerAction?.Invoke(service);
                    _timerActionAsync?.Invoke(service)?.Wait(_cancellationTokenSource!.Token);
                    _timerActionWithCancelationToken?.Invoke(service, _cancellationTokenSource!.Token);
                    _timerActionWithCancelationTokenAsync?.Invoke(service, _cancellationTokenSource!.Token)?.Wait();
                }
                finally
                {
                    _semaphore.Release();
                    logger?.LogTrace("Done.");
                }
            }
            else
            {
                logger?.LogWarning("{TypeOfTimer} was not triggered because it reached the concurrency limit.", typeof(T).Name);
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
