using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;
public class SignUpDto
{
	[EmailAddress]
	[Required]
	public string Email { get; internal set; }

	[Required]
	public string Password { get; internal set; }

	public string Role { get; internal set; }

	public Dictionary<string, IEnumerable<string>> Claims { get; set; }
}
