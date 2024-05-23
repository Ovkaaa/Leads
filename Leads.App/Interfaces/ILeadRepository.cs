using Leads.Domain.Entities;

namespace Leads.App.Interfaces;

public interface ILeadRepository
{
    Task<Lead> GetByIdAsync(long id);
    Task CreateAsync(Lead lead);
    Task UpdateAsync(Lead lead);
    Task SoftDeleteAsync(long id);
    Task<IEnumerable<Lead>> ListAsync(int page, int pageSize);
}
