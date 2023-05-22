using rmdev.ScopedTimerHostedService;

public class ExampleTimerA : IScopedTimer
{
    private readonly ILogger<ExampleTimerA> _logger;

    public ExampleTimerA(ILogger<ExampleTimerA> logger)
    {
        _logger = logger;
        _logger.LogInformation("Creating a ExampleTimerA instance...");
    }

    public void Do(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Doing something...");
        Thread.Sleep(1500);
    }
}
