using System.Threading;

namespace rmdev.ScopedTimerHostedService
{
    /// <summary>
    /// Scoped timer contract
    /// </summary>
    public interface IScopedTimer : IIntervaledTimer
    {
        /// <summary>
        /// Do something when time elapsed
        /// </summary>
        /// <param name="cancellationToken">Cancellation is requested when the service is stopping</param>
        void Do(CancellationToken cancellationToken);
    }
}
