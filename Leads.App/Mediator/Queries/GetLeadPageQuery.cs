using Leads.App.Interfaces;
using Leads.Domain.Entities;
using MediatR;

namespace Leads.App.Mediator.Queries;

public class GetLeadPageQuery : IRequest<IEnumerable<Lead>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public class GetLeadPageQueryHandler(ILeadRepository leadRepository) : IRequestHandler<GetLeadPageQuery, IEnumerable<Lead>>
    {
        private readonly ILeadRepository leadRepository = leadRepository;

        public async Task<IEnumerable<Lead>> Handle(GetLeadPageQuery request, CancellationToken cancellationToken)
        {
            return await leadRepository.ListAsync(request.PageNumber, request.PageSize);
        }
    }
}
