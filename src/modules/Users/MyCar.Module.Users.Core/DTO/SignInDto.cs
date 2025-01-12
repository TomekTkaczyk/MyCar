﻿using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;

public class SignInDto
{
	[Required]
	[MinLength(3), MaxLength(20)]
	public string Identifier { get; set; }

	public string Password { get; set; }
}
