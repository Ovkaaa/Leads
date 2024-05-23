namespace Leads.App.Exceptions;

public class ExsistLeadException(long id) : Exception($"Lead with Id {id} alreary exist.")
{
}
