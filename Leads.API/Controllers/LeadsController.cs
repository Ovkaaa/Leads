using AutoMapper;
using Leads.App.Contracts.Request;
using Leads.App.Mediator.Commands;
using Leads.App.Mediator.Queries;
using Leads.App.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Leads.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator mediator = mediator;
    private readonly IMapper mapper = mapper;

    /// <summary>
    /// Get Lead by Id
    /// </summary>
    /// <param name="id">Lead Id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<LeadDto>> GetLeadById(long id)
    {
        var lead = await mediator.Send(new GetLeadByIdQuery { Id = id });
        if (lead == null)
            return NotFound();

        var result = mapper.Map<LeadDto>(lead);

        return Ok(result);
    }

    /// <summary>
    /// Get Leads page
    /// </summary>
    /// <param name="query">Query page size and page number</param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<ActionResult<LeadDto>> GetLeadsPage(GetLeadPageQuery query)
    {
        var leads = await mediator.Send(query);

        var result = mapper.Map<IEnumerable<LeadDto>>(leads);

        return Ok(result);
    }

    /// <summary>
    /// Create new Lead
    /// </summary>
    /// <param name="request">Lead data</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<long>> CreateLead(LeadModelRequest request)
    {
        var leadDto = mapper.Map<LeadDto>(request);

        var result = await mediator.Send(new CreateLeadCommand { LeadDto = leadDto });

        return Ok(result);
    }

    /// <summary>
    /// Update Lead
    /// </summary>
    /// <param name="id">Lead Id</param>
    /// <param name="request">Lead data</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLead(long id, [FromBody] LeadModelRequest request)
    {
        var leadDto = mapper.Map<LeadDto>(request);
        leadDto.Id = id;

        await mediator.Send(new UpdateLeadCommand { LeadDto = leadDto });

        return Ok();
    }


    /// <summary>
    /// Delete Lead
    /// </summary>
    /// <param name="id">Lead Id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLead(long id)
    {
        await mediator.Send(new DeleteLeadCommand { LeadId = id });
        return NoContent();
    }
}