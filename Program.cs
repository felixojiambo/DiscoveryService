using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Consul;

var builder = WebApplication.CreateBuilder(args);

// Retrieve Consul configuration from environment variables or fallback defaults.
string consulAddress = builder.Configuration["Consul:Address"] ?? "http://127.0.0.1:8500";

// Register the Consul client as a singleton.
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    cfg.Address = new Uri(consulAddress);
}));

var app = builder.Build();

// Define service registration details.
// In production, values such as Address and Port should be obtained from configuration.
var serviceId = "DiscoveryService-" + Guid.NewGuid();
var registration = new AgentServiceRegistration
{
    ID = serviceId,
    Name = "DiscoveryService",
    Address = "localhost", // Use the actual IP address or hostname in production.
    Port = 5000,           // Ensure this matches the port your service listens on.
    Check = new AgentServiceCheck
    {
        HTTP = "http://localhost:5000/health",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5),
        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1)
    }
};

// Register the service with Consul when the application starts.
var lifetime = app.Lifetime;
var consulClient = app.Services.GetRequiredService<IConsulClient>();

lifetime.ApplicationStarted.Register(() =>
{
    Task.Run(async () =>
    {
        await consulClient.Agent.ServiceRegister(registration);
        Console.WriteLine("Service registered with Consul.");
    });
});

// Deregister the service when the application stops.
lifetime.ApplicationStopping.Register(() =>
{
    Task.Run(async () =>
    {
        await consulClient.Agent.ServiceDeregister(registration.ID);
        Console.WriteLine("Service deregistered from Consul.");
    });
});

// Health check endpoint for Consul.
app.MapGet("/health", () => Results.Ok("Healthy"));

// Endpoint to query all registered services from the local Consul agent.
app.MapGet("/services", async () =>
{
    var services = await consulClient.Agent.Services();
    return Results.Ok(services.Response);
});

app.Run();
