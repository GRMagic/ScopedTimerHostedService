public class MyServiceB
{
    private readonly ILogger<MyServiceB> _logger;

    public MyServiceB(ILogger<MyServiceB> logger)
    {
        _logger = logger;
        _logger.LogInformation("Creating an MyServiceB instance...");
    }

    public void DoSomething(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Doing something realy slow...");
        Task.Delay(10_000, cancellationToken).Wait();
        _logger.LogInformation("Done something realy slow.");
    }
}