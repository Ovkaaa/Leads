namespace Leads.App.Contracts.Request;

public class LeadModelRequest
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IpAddress { get; set; }
    public string CountryCode { get; set; }
    public long AffiliateId { get; set; }
}
