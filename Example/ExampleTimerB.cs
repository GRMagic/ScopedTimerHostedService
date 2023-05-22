using rmdev.ScopedTimerHostedService;

public class ExampleTimerB : IScopedTimerAsync
{
    private readonly ILogger<ExampleTimerB> _logger;

    public ExampleTimerB(ILogger<ExampleTimerB> logger)
    {
        _logger = logger;
        _logger.LogInformation("Creating a ExampleTimerB instance...");
    }

    public async Task DoAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Doing something...");
        await Task.Delay(10000, cancellationToken);
    }
}
