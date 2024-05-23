using Leads.App.Exceptions;
using Leads.App.Interfaces;
using MediatR;

namespace Leads.App.Mediator.Commands;

public class DeleteLeadCommand : IRequest<Unit>
{
    public long LeadId { get; set; }

    public class DeleteLeadCommandHandler(ILeadRepository leadRepository) : IRequestHandler<DeleteLeadCommand, Unit>
    {
        private readonly ILeadRepository leadRepository = leadRepository;

        public async Task<Unit> Handle(DeleteLeadCommand request, CancellationToken cancellationToken)
        {
            await leadRepository.SoftDeleteAsync(request.LeadId);
            return Unit.Value;
        }
    }
}
