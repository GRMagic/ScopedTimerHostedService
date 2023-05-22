public class MyServiceA
{
    private readonly ILogger<MyServiceA> _logger;

    public MyServiceA(ILogger<MyServiceA> logger)
    {
        _logger = logger;
        _logger.LogInformation("Creating an MyServiceA instance...");
    }

    public void DoSomething()
    {
        _logger.LogInformation("Doing something slow...");
        Thread.Sleep(2000);
    }

    public async Task DoSomethingAsync()
    {
        _logger.LogInformation("Doing something async...");
        await Task.Delay(2000);
    }

    public void DoOtherThing(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Doing other thing...");
        Task.Delay(10_000, cancellationToken).Wait();
    }

    public async Task DoOtherThingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Doing other thing async...");
        await Task.Delay(10_000);
    }
}
