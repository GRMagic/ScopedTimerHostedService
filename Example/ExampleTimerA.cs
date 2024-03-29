﻿using rmdev.ScopedTimerHostedService;

public class ExampleTimerA : IScopedTimer
{
    private readonly ILogger<ExampleTimerA> _logger;

    public ExampleTimerA(ILogger<ExampleTimerA> logger)
    {
        _logger = logger;
        _logger.LogInformation("Creating a ExampleTimerA instance...");
    }

    public double Interval => Random.Shared.Next(100, 10000);

    public void Do(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Doing something...");
        Thread.Sleep(1500);
    }
}
