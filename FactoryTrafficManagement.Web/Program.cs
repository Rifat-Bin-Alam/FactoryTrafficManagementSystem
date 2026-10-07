using System.Text.Json.Serialization;
using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Application.Services;
using FactoryTrafficManagement.Infrastructure.Controllers;
using FactoryTrafficManagement.Infrastructure.Persistence;
using FactoryTrafficManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IJunctionRepository, JunctionRepository>();
builder.Services.AddScoped<IJunctionService, JunctionService>();

builder.Services.AddScoped<ISensorEventRepository, SensorEventRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ISensorEventService, SensorEventService>();

builder.Services.AddScoped<
    ITrafficDecisionService,
    TrafficDecisionService>();

builder.Services.AddScoped<
    IAutomaticTrafficService,
    AutomaticTrafficService>();

builder.Services.AddScoped<
    IControllerCommandRepository,
    ControllerCommandRepository>();

builder.Services.AddScoped<
    IControllerCommandService,
    ControllerCommandService>();

builder.Services.AddScoped<
    IControllerGateway,
    SimulatedControllerGateway>();

builder.Services.AddScoped<
    IControllerEventService,
    ControllerEventService>();

builder.Services.AddScoped<
    ITrafficTransitionService,
    TrafficTransitionService>();
builder.Services.AddScoped<
    IControllerStatusService,
    ControllerStatusService>();

builder.Services.AddScoped<
    IAuditRepository,
    AuditRepository>();

// MVC + JSON enum serialization
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await DbInitializer.InitializeAsync(dbContext);
}

// HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();