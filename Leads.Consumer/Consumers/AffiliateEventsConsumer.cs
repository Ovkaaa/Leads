using Leads.App.Events;
using Leads.App.Exceptions;
using Leads.App.Interfaces;
using Leads.Domain.Entities;
using Leads.Domain.Enums;
using MassTransit;

namespace Leads.Consumer.Consumers;

public class AffiliateEventsConsumer(ILeadRepository leadRepository) : IConsumer<AffiliateCreatedEvent>, IConsumer<AffiliateUpdatedEvent>
{
    private readonly ILeadRepository leadRepository = leadRepository;

    public async Task Consume(ConsumeContext<AffiliateCreatedEvent> context)
    {
        await leadRepository.CreateAsync(new Lead
        {
            Id = context.Message.Id,
            Email = context.Message.Email,
            FirstName = context.Message.AccountName,
            CreatedOn = DateTime.UtcNow
        });
    }

    public async Task Consume(ConsumeContext<AffiliateUpdatedEvent> context)
    {
        if (context.Message.Status == AffiliateStatus.Blocked)
        {
            await leadRepository.SoftDeleteAsync(context.Message.Id);
            return;
        }

        var lead = await leadRepository.GetByIdAsync(context.Message.Id) ?? throw new LeadNotFoundException(context.Message.Id);

        lead.FirstName = context.Message.AccountName;
        lead.Email = context.Message.Email;

        await leadRepository.UpdateAsync(lead);
    }
}
