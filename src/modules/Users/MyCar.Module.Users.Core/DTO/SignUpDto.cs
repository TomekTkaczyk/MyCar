using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;
public class SignUpDto
{
	[Required]
	public string Name { get; set; }

	[EmailAddress]
	[Required]
	public string Email { get; set; }

	[Required]
	public string Password { get; set; }
}
