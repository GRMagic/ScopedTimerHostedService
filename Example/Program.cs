using rmdev.ScopedTimerHostedService;

var builder = WebApplication.CreateBuilder(args);

// Dependencies
//builder.Services.AddScoped<ExampleTimerA>();
builder.Services.AddScoped<ExampleTimerB>();
builder.Services.AddScoped<MyServiceA>();
builder.Services.AddScoped<MyServiceB>();

// Just one timer per injected type service

// Examples (better to test one at a time)
//builder.Services.AddScopedTimer<ExampleTimerA>(1000);
//builder.Services.AddScopedTimer<ExampleTimerA>();
//builder.Services.AddScopedTimerAsync<ExampleTimerB>(1000);
//builder.Services.AddScopedTimer<MyServiceA>(myService => myService.DoSomething(), 5000);
//builder.Services.AddScopedTimer<MyServiceA>((myService, cancellationToken) => myService.DoOtherThing(cancellationToken), 3000);
//builder.Services.AddScopedTimer<MyServiceB>((myService, cancellationToken) => myService.DoSomething(cancellationToken), 3000, 2);
//builder.Services.AddScopedTimer<MyServiceA>(myService => myService.DoSomethingAsync(), 1000);
//builder.Services.AddScopedTimer<MyServiceA>((myService, cancellationToken) => myService.DoOtherThingAsync(cancellationToken), 1000);

// ... 
var app = builder.Build();
app.MapGet("/", () => "Look to the console");
app.Run();
