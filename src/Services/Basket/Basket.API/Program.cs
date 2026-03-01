using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Microsoft.EntityFrameworkCore;
using Carter;
using Basket.API.Repositories;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

builder.Services.AddDbContext<BasketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseExceptionHandler(options => { });

app.Run();