using Leads.Domain.Enums;

namespace Leads.App.Events;

public abstract class AffiliateEventBase
{
    public int Id { get; set; }
    public string AccountName { get; set; }
    public string Email { get; set; }
    public abstract AffiliateStatus Status { get; set; }
}
