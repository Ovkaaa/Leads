using AutoMapper;
using Leads.App.Contracts.Request;
using Leads.App.Models;
using Leads.Domain.Entities;

namespace Leads.API.Infrastructure.MappingProfiles;

public class LeadProfile : Profile
{
    public LeadProfile()
    {
        CreateMap<LeadModelRequest, LeadDto>(MemberList.Source);
        CreateMap<Lead, LeadDto>(MemberList.Source);
    }
}
