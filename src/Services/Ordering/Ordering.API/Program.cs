using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// General Configurations
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// builder.Services.AddScoped<BasketCheckoutConsumer>();


// // MassTransit Configuration
// builder.Services.AddMassTransit(config =>
// {
//     config.AddConsumer<BasketCheckoutConsumer>();
//     config.UsingRabbitMq((ctx, cfg) =>
//     {
//         cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
//
//         cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
//         {
//             c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
//         });
//     });
// });
// builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed
        .SeedAsync(context, logger)
        .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();