using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;

public class SignInDto
{
	[Required]
	public string Identifier { get; set; }

	[Required]
	public string Password { get; set; }
}
