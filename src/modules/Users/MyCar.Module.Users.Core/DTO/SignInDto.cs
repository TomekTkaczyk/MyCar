using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;
public class SignInDto
{
	[EmailAddress]
	[Required]
	public string Email { get; internal set; }

	[Required]
	public string Password { get; internal set; }
}
