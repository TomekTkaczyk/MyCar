using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;
public class UserDetailsDto : UserDto
{
	[StringLength(1000)]
	public string Description { get; set; }
}
