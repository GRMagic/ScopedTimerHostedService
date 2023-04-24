using System.Threading;

namespace rmdev.ScopedTimerHostedService
{
    public interface IScopedTimer
    {
        void Do(CancellationToken cancellationToken);
    }
}
