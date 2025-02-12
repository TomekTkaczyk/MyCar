﻿using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.UseCases.Commands.ChangePassword;
internal sealed record ChangePasswordRequest
{
	[Required]
	public string CurrentPassword { get; set; }

	[Required]
	public string Password { get; set; }

}
