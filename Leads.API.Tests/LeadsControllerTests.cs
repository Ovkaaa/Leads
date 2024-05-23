using AutoMapper;
using Leads.Api.Controllers;
using Leads.App.Contracts.Request;
using Leads.App.Mediator.Commands;
using Leads.App.Mediator.Queries;
using Leads.App.Models;
using Leads.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Leads.API.Tests;

[TestClass]
public class LeadsControllerTests
{
    private LeadsController controller;
    private Mock<IMediator> mediatorMock;
    private Mock<IMapper> mapperMock;

    [TestInitialize]
    public void Setup()
    {
        mediatorMock = new Mock<IMediator>();
        mapperMock = new Mock<IMapper>();

        controller = new LeadsController(mediatorMock.Object, mapperMock.Object);
    }

    [TestMethod]
    public async Task GetLeadById_ExistingLead_ReturnsOkResult()
    {
        // Arrange
        long leadId = 1;
        var lead = new Lead { Id = leadId };
        var leadDto = new LeadDto { Id = leadId };
        mediatorMock
            .Setup(m => m.Send(It.Is<GetLeadByIdQuery>(q => q.Id.Equals(leadId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lead)
            .Verifiable(Times.Once);
        mapperMock
            .Setup(m => m.Map<LeadDto>(It.IsAny<Lead>()))
            .Returns(leadDto)
            .Verifiable(Times.Once);

        // Act
        var result = await controller.GetLeadById(leadId);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual(leadDto, ((OkObjectResult)result.Result).Value);
    }

    [TestMethod]
    public async Task GetLeadById_NonExistingLead_ReturnsNotFoundResult()
    {
        // Arrange
        long leadId = 1;
        mediatorMock
            .Setup(m => m.Send(It.Is<GetLeadByIdQuery>(q => q.Id.Equals(leadId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Lead)null)
            .Verifiable(Times.Once);

        // Act
        var result = await controller.GetLeadById(leadId);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task GetLeadsPage_ReturnsOkResult()
    {
        // Arrange
        var leads = new List<Lead> { new() { Id = 1 }, new() { Id = 2 } };
        var leadsDto = new List<LeadDto> { new() { Id = leads[0].Id }, new() { Id = leads[1].Id } };
        var query = new GetLeadPageQuery();
        mediatorMock
            .Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(leads.AsEnumerable())
            .Verifiable(Times.Once);
        mapperMock
            .Setup(m => m.Map<IEnumerable<LeadDto>>(It.IsAny<IEnumerable<Lead>>()))
            .Returns(leadsDto)
            .Verifiable(Times.Once);

        // Act
        var result = await controller.GetLeadsPage(query);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual(leadsDto, ((OkObjectResult)result.Result).Value);
    }

    [TestMethod]
    public async Task CreateLead_ValidLead_ReturnsOkResult()
    {
        // Arrange
        var request = new LeadModelRequest
        {
            Email = "test@example.com",
            Phone = "123-12-12",
            FirstName = "Test_FirstName",
            LastName = "Test_LastName",
            IpAddress = "111",
            CountryCode = "test",
            AffiliateId = 1
        };
        var leadDto = new LeadDto
        {
            Email = request.Email,
            Phone = request.Phone,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IpAddress = request.IpAddress,
            CountryCode = request.CountryCode,
            AffiliateId = request.AffiliateId
        };
        long newLeadId = 1;
        mapperMock
            .Setup(m => m.Map<LeadDto>(request))
            .Returns(leadDto)
            .Verifiable(Times.Once);
        mediatorMock
            .Setup(m => m.Send(It.Is<CreateLeadCommand>(c => c.LeadDto.Equals(leadDto)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newLeadId)
            .Verifiable(Times.Once);

        // Act
        var result = await controller.CreateLead(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual(newLeadId, ((OkObjectResult)result.Result).Value);
    }

    [TestMethod]
    public async Task UpdateLead_ValidLead_ReturnsOkResult()
    {
        // Arrange
        long leadId = 1;
        var request = new LeadModelRequest();
        var leadDto = new LeadDto { Id = leadId };
        mapperMock
            .Setup(m => m.Map<LeadDto>(request))
            .Returns(leadDto)
            .Verifiable(Times.Once);
        mediatorMock
            .Setup(m => m.Send(
                It.Is<UpdateLeadCommand>(c => c.LeadDto.Equals(leadDto) && leadDto.Id.Equals(leadId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value)
            .Verifiable(Times.Once);

        // Act
        var result = await controller.UpdateLead(leadId, request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public async Task DeleteLead_ExistingLead_ReturnsNoContentResult()
    {
        // Arrange
        long leadId = 1;
        mediatorMock
            .Setup(m => m.Send(It.Is<DeleteLeadCommand>(c => c.LeadId.Equals(leadId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value)
            .Verifiable(Times.Once);

        // Act
        var result = await controller.DeleteLead(leadId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}