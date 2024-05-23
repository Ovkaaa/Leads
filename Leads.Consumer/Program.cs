using Leads.App.Interfaces;
using Leads.Consumer.Consumers;
using Leads.Consumer.Infrastructure.Models;
using Leads.Infrastructure.Repositories;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();

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
