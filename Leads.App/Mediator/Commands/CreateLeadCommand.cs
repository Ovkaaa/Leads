using Leads.App.Interfaces;
using Leads.App.Models;
using Leads.Domain.Entities;
using MediatR;

namespace Leads.App.Mediator.Commands;

public class CreateLeadCommand : IRequest<long>
{
    public LeadDto LeadDto { get; set; }

    public class CreateLeadCommandHandler(ILeadRepository leadRepository) : IRequestHandler<CreateLeadCommand, long>
    {
        private readonly ILeadRepository leadRepository = leadRepository;

        public async Task<long> Handle(CreateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = new Lead
            {
                Email = request.LeadDto.Email,
                Phone = request.LeadDto.Phone,
                FirstName = request.LeadDto.FirstName,
                LastName = request.LeadDto.LastName,
                IpAddress = request.LeadDto.IpAddress,
                CountryCode = request.LeadDto.CountryCode,
                AffiliateId = request.LeadDto.AffiliateId,
                CreatedOn = DateTime.UtcNow
            };

            await leadRepository.CreateAsync(lead);
            return lead.Id;
        }
    }
}
