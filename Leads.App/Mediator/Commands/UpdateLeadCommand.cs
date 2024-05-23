using Leads.App.Exceptions;
using Leads.App.Interfaces;
using Leads.App.Models;
using MediatR;

namespace Leads.App.Mediator.Commands;

public class UpdateLeadCommand : IRequest<Unit>
{
    public LeadDto LeadDto { get; set; }

    public class UpdateLeadCommandHandler(ILeadRepository leadRepository) : IRequestHandler<UpdateLeadCommand, Unit>
    {
        private readonly ILeadRepository leadRepository = leadRepository;

        public async Task<Unit> Handle(UpdateLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await leadRepository.GetByIdAsync(request.LeadDto.Id!.Value) ?? throw new LeadNotFoundException(request.LeadDto.Id!.Value);

            lead.Email = request.LeadDto.Email;
            lead.Phone = request.LeadDto.Phone;
            lead.FirstName = request.LeadDto.FirstName;
            lead.LastName = request.LeadDto.LastName;
            lead.IpAddress = request.LeadDto.IpAddress;
            lead.CountryCode = request.LeadDto.CountryCode;
            lead.AffiliateId = request.LeadDto.AffiliateId;

            await leadRepository.UpdateAsync(lead);
            return Unit.Value;
        }
    }
}
