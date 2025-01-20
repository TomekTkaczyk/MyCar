namespace MyCar.Module.Users.Core.DTO;

public class AccountDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Role { get; set; }
	public IDictionary<string, IEnumerable<string>> Claims { get; set; }
	public bool IsConfirmed { get; set; }
}