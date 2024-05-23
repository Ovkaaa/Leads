using Leads.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Leads.Infrastructure;

public class LeadsDbContext(DbContextOptions<LeadsDbContext> options) : DbContext(options)
{
    public DbSet<Lead> Leads { get; set; }
}
