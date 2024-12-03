namespace MyCar.Module.Users.Core.Entities;

public class User
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public string Email { get; set; }
	public string Name { get; set; }
	public string Password { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Role { get; set; }
	public bool IsActive { get; set; }
	public Dictionary<string, IEnumerable<string>> Claims { get; set; }
	public bool EmailConfirm { get; set; }
	public string EmailConfirmToken { get; set; }
	public DateTime EmailConfirmExpires { get; set; }
	public string RefreshToken { get; set; }
	public DateTime RefreshExpires { get; set; }

}
