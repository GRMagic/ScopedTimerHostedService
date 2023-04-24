using Microsoft.Extensions.DependencyInjection;

namespace rmdev.ScopedTimerHostedService
{
    public static class ServiceCollectionExtensions
    {
        public static void AddScopedTimer<T>(this IServiceCollection services, double interval, int concurrent = 1) where T : IScopedTimer
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<T>(sp, interval, concurrent, null, (s, c) => s.Do(c)));
        }

        public static void AddScopedTimer<TService>(this IServiceCollection services, TimerAction<TService> action, double interval, int concurrent = 1) where TService : notnull
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<TService>(sp, interval, concurrent, action));
        }

        public static void AddScopedTimer<TService>(this IServiceCollection services, TimerActionWithCancelationToken<TService> action, double interval, int concurrent = 1) where TService : notnull
        {
            services.AddHostedService(sp => new ScopedTimerHostedService<TService>(sp, interval, concurrent, null, action));
        }
    }
}
