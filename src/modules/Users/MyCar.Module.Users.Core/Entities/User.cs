using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyCar.Shared.Abstractions.Entities;

namespace MyCar.Module.Users.Core.Entities;

public class User : EntityBase
{
	public string Email { get; set; }
	public string Name { get; set; }
	public string Password { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Role { get; set; }
	public bool IsActive { get; set; }
	public IDictionary<string, IEnumerable<string>> Claims { get; set; }
	public bool EmailConfirm { get; set; }
	public string EmailConfirmToken { get; set; }
	public DateTime EmailConfirmExpires { get; set; }
	public string RefreshToken { get; set; }
	public DateTime RefreshExpires { get; set; }

}
