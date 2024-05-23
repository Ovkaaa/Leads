using Leads.Domain.Enums;

namespace Leads.App.Exceptions;

public class InvalidAffiliateEventStatusException(string eventName, AffiliateStatus status) : Exception($"Attemd to set unexpected status on {eventName}. Status: {status}")
{
}
