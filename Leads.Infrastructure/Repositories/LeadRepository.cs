using Leads.App.Exceptions;
using Leads.App.Interfaces;
using Leads.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leads.Infrastructure.Repositories;

public class LeadRepository(LeadsDbContext context) : ILeadRepository
{
    private readonly LeadsDbContext context = context;

    public async Task<Lead> GetByIdAsync(long id)
    {
        return await context.Leads.FindAsync(id);
    }

    public async Task CreateAsync(Lead lead)
    {
        if (await GetByIdAsync(lead.Id) is not null)
            throw new ExsistLeadException(lead.Id);

        context.Leads.Add(lead);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lead lead)
    {
        context.Leads.Update(lead);
        await context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(long id)
    {
        var lead = await context.Leads.FindAsync(id) ?? throw new LeadNotFoundException(id);
        lead.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Lead>> ListAsync(int pageNumber, int pageSize)
    {
        var query = context.Leads.AsQueryable();
        return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}
