using Leads.App.Interfaces;
using Leads.Consumer.Consumers;
using Leads.Consumer.Infrastructure.Models;
using Leads.Infrastructure;
using Leads.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();

builder.Services.AddDbContext<LeadsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });

        cfg.ReceiveEndpoint(rabbitMqSettings.QueueName, e =>
        {
            e.Consumer<AffiliateEventsConsumer>(context);
        });
    });
});
builder.Services.AddHostedService<MassTransitHostedService>();

builder.Services.AddSingleton<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<AffiliateEventsConsumer>();

var host = builder.Build();
host.Run();
