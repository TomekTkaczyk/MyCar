using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;
public class ChangePasswordDto
{
	[Required]
	public string CurrentPassword { get; set; }

	[Required]
	public string Password { get; set; }
}
