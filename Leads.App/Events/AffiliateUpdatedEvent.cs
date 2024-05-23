using Leads.App.Exceptions;
using Leads.Domain.Enums;

namespace Leads.App.Events;

public class AffiliateUpdatedEvent : AffiliateEventBase
{
    private AffiliateStatus status;
    public override AffiliateStatus Status
    {
        get => status;
        set
        {
            if (value is not AffiliateStatus.Active and not AffiliateStatus.Pending and not AffiliateStatus.Blocked)
            {
                throw new InvalidAffiliateEventStatusException(nameof(AffiliateUpdatedEvent), value);
            }

            status = value;
        }
    }
}
