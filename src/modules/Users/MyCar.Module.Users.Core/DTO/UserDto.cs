using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;
public class UserDto
{
	public Guid Id { get; set; }

	[Required]
	[StringLength(100, MinimumLength = 3)]
	public string Firstname { get; set; }

	[Required]
	[StringLength(100, MinimumLength = 3)]
	public string Lastname { get; set; }
}
