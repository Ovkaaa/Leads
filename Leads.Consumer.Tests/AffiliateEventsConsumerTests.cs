using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MassTransit;
using Leads.App.Interfaces;
using Leads.Consumer.Consumers;
using Leads.App.Events;
using Leads.Domain.Entities;
using Leads.Domain.Enums;

[TestClass]
public class AffiliateEventsConsumerTests
{
    private Mock<ILeadRepository> leadRepositoryMock;
    private AffiliateEventsConsumer consumer;

    [TestInitialize]
    public void Setup()
    {
        leadRepositoryMock = new Mock<ILeadRepository>();

        consumer = new AffiliateEventsConsumer(leadRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Consume_AffiliateCreatedEvent_CreatesLead()
    {
        // Arrange
        var createdEvent = new AffiliateCreatedEvent
        {
            Id = 1,
            Email = "test@example.com",
            AccountName = "Test Affiliate"
        };

        // Act
        await consumer.Consume(CreateConsumeContextMock(createdEvent));

        // Assert
        leadRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Lead>(lead =>
            lead.Id == createdEvent.Id &&
            lead.Email == createdEvent.Email &&
            lead.FirstName == createdEvent.AccountName
        )), Times.Once);
    }

    [TestMethod]
    public async Task Consume_AffiliateUpdatedEvent_BlockedStatus_SoftDeletesLead()
    {
        // Arrange
        var updatedEvent = new AffiliateUpdatedEvent
        {
            Id = 1,
            Email = "test@example.com",
            AccountName = "Test Affiliate",
            Status = AffiliateStatus.Blocked
        };
        leadRepositoryMock
            .Setup(repo => repo.SoftDeleteAsync(updatedEvent.Id))
            .Verifiable(Times.Once);

        // Act
        await consumer.Consume(CreateConsumeContextMock(updatedEvent));
    }

    [TestMethod]
    public async Task Consume_AffiliateUpdatedEvent_NonBlockedStatus_UpdatesLead()
    {
        // Arrange
        var updatedEvent = new AffiliateUpdatedEvent
        {
            Id = 1,
            Email = "test@example.com",
            AccountName = "Test Affiliate",
            Status = AffiliateStatus.Active
        };
        var lead = new Lead { Id = updatedEvent.Id };
        leadRepositoryMock
            .Setup(repo => repo.GetByIdAsync(updatedEvent.Id))
            .ReturnsAsync(lead)
            .Verifiable(Times.Once);

        // Act
        await consumer.Consume(CreateConsumeContextMock(updatedEvent));

        // Assert
        leadRepositoryMock.Verify(repo => repo.UpdateAsync(lead), Times.Once);
        Assert.AreEqual(updatedEvent.Email, lead.Email);
        Assert.AreEqual(updatedEvent.AccountName, lead.FirstName);
    }

    private ConsumeContext<TAffiliateEvent> CreateConsumeContextMock<TAffiliateEvent>(TAffiliateEvent affiliateEvent) where TAffiliateEvent : AffiliateEventBase
    {
        var consumeContextMock = new Mock<ConsumeContext<TAffiliateEvent>>();
        consumeContextMock
            .SetupGet(context => context.Message)
            .Returns(affiliateEvent);

        return consumeContextMock.Object;
    }
}
