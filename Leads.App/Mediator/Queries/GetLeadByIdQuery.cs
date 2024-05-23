using Leads.App.Interfaces;
using Leads.Domain.Entities;
using MediatR;

namespace Leads.App.Mediator.Queries;

public class GetLeadByIdQuery : IRequest<Lead>
{
    public long Id { get; set; }

    public class GetLeadByIdQueryHandler(ILeadRepository leadRepository) : IRequestHandler<GetLeadByIdQuery, Lead>
    {
        private readonly ILeadRepository leadRepository = leadRepository;

        public async Task<Lead> Handle(GetLeadByIdQuery request, CancellationToken cancellationToken)
        {
            return await leadRepository.GetByIdAsync(request.Id);
        }
    }
}
