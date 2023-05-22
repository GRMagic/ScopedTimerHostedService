# rmdev.ScopedTimerHostedService

[![NuGet](https://img.shields.io/nuget/v/rmdev.scopedtimerhostedservice.svg)](https://nuget.org/packages/rmdev.scopedtimerhostedservice)
[![Nuget](https://img.shields.io/nuget/dt/rmdev.scopedtimerhostedservice.svg)](https://nuget.org/packages/rmdev.scopedtimerhostedservice) 

Library to allow adding timers with scoped lifecycle and concurrency control.

## Examples

```csharp
using rmdev.ScopedTimerHostedService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IService, Service>();
builder.Services.AddHostedTimer<IService>(service => service.DoSomething(), 5000);

// ... 

```
Check example project for more detailed examples
