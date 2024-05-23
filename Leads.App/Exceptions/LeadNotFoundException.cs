namespace Leads.App.Exceptions;

public class LeadNotFoundException(long id) : Exception($"Lead with id {id} is not found.")
{
}
