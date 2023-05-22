using Microsoft.Extensions.DependencyInjection;

namespace rmdev.ScopedTimerHostedService
{
    /// <remarks/>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a timer that use execution scope
        /// </summary>
        /// <typeparam name="T">An implementation of IScopedTimer</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="interval">Execution interval in ms</param>
        /// <param name="concurrent">Max number of concurrent executions</param>
        public static void AddScopedTimer<T>(this IServiceCollection services, double interval, int concurrent = 1) where T : IScopedTimer
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<T>(sp, interval, concurrent, (s, c) => s.Do(c)));
        }

        /// <summary>
        /// Add a timer that use execution scope
        /// </summary>
        /// <typeparam name="T">An implementation of IScopedTimer</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="interval">Execution interval in ms</param>
        /// <param name="concurrent">Max number of concurrent executions</param>
        public static void AddScopedTimerAsync<T>(this IServiceCollection services, double interval, int concurrent = 1) where T : IScopedTimerAsync
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<T>(sp, interval, concurrent, (s, c) => s.DoAsync(c)));
        }

        /// <summary>
        /// Add a timer that use execution scope
        /// </summary>
        /// <typeparam name="TService">Injected service type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="action">The action to execute (without cancelation token)</param>
        /// <param name="interval">Execution interval in ms</param>
        /// <param name="concurrent">Max number of concurrent executions</param>
        public static void AddScopedTimer<TService>(this IServiceCollection services, TimerAction<TService> action, double interval, int concurrent = 1) where TService : notnull
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<TService>(sp, interval, concurrent, action));
        }

        /// <summary>
        /// Add a timer that use execution scope
        /// </summary>
        /// <typeparam name="TService">Injected service type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="action">The action to execute (without cancelation token)</param>
        /// <param name="interval">Execution interval in ms</param>
        /// <param name="concurrent">Max number of concurrent executions</param>
        public static void AddScopedTimer<TService>(this IServiceCollection services, TimerActionAsync<TService> action, double interval, int concurrent = 1) where TService : notnull
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<TService>(sp, interval, concurrent, action));
        }

        /// <summary>
        /// Add a timer that use execution scope
        /// </summary>
        /// <typeparam name="TService">Injected service type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="action">The action to execute (with cancelation token)</param>
        /// <param name="interval">Execution interval in ms</param>
        /// <param name="concurrent">Max number of concurrent executions</param>
        public static void AddScopedTimer<TService>(this IServiceCollection services, TimerActionWithCancelationToken<TService> action, double interval, int concurrent = 1) where TService : notnull
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<TService>(sp, interval, concurrent, action));
        }

        /// <summary>
        /// Add a timer that use execution scope
        /// </summary>
        /// <typeparam name="TService">Injected service type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="action">The action to execute (with cancelation token)</param>
        /// <param name="interval">Execution interval in ms</param>
        /// <param name="concurrent">Max number of concurrent executions</param>
        public static void AddScopedTimerAsync<TService>(this IServiceCollection services, TimerActionWithCancelationTokenAsync<TService> action, double interval, int concurrent = 1) where TService : notnull
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<TService>(sp, interval, concurrent, action));
        }
    }
}
