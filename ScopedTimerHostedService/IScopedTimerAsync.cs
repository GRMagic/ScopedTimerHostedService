using System.Threading;
using System.Threading.Tasks;

namespace rmdev.ScopedTimerHostedService
{
    /// <summary>
    /// Scoped timer contract async
    /// </summary>
    public interface IScopedTimerAsync
    {
        /// <summary>
        /// Do something when time elapsed
        /// </summary>
        Task DoAsync(CancellationToken cancellationToken);
    }

}
