namespace Leads.App.Models;

public class LeadDto
{
    public long? Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IpAddress { get; set; }
    public string CountryCode { get; set; }
    public long AffiliateId { get; set; }
}
