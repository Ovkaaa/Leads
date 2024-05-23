using Leads.App.Interfaces;
using Leads.App.Mediator.Commands;
using Leads.Infrastructure;
using Leads.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(cfg => { cfg.AllowNullCollections = true; });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LeadsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LeadsConnection")));

builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddMediatR((config) => config.RegisterServicesFromAssembly(typeof(CreateLeadCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();