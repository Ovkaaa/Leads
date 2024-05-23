using Leads.App.Exceptions;
using Leads.Domain.Enums;

namespace Leads.App.Events;

public class AffiliateCreatedEvent : AffiliateEventBase
{
    public override AffiliateStatus Status
    {
        get => AffiliateStatus.Active;
        set
        {
            if (value is not AffiliateStatus.Active)
            {
                throw new InvalidAffiliateEventStatusException(nameof(AffiliateCreatedEvent), value);
            }
        }
    }
}
