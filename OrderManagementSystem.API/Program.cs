using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Services;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Infrastructure.Repositories;
using Serilog;
using AutoMapper;
using OrderManagementSystem.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// --------------------
// Serilog Configuration
// --------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// --------------------
// Add services
// --------------------
builder.Services.AddControllers();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();

// AutoMapper Configuration (15.x sürümü ile uyumlu)
//builder.Services.AddAutoMapper(cfg =>
//{
//    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
//});


builder.Services.AddAutoMapper((serviceProvider, cfg) =>
{
    cfg.AddProfile<OrderManagementSystem.Application.Mapping.MappingProfile>();
}, typeof(Program).Assembly);




// --------------------
// Swagger Configuration
// --------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OrderManagement API",
        Version = "v1",
        Description = "Order Management System API documentation"
    });
});

// --------------------
// Build and Configure Pipeline
// --------------------
var app = builder.Build();

// SEED DATA
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.Initialize(services);
        Console.WriteLine("Seed data tamamlandý.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seed data hatasý: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderManagement API v1");
        c.RoutePrefix = string.Empty; // Swagger UI root sayfada açýlýr
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseCors("AllowAll");

app.Run();
