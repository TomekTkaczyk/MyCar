﻿using System.ComponentModel.DataAnnotations;

namespace MyCar.Module.Users.Core.DTO;
public sealed record ConfirmEmailDto
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string ConfirmToken { get; set; }
}
