﻿using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.DTO;
public class UserDto
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Role { get; set; }
	public IDictionary<string, IEnumerable<string>> Claims { get; set; }
	public bool IsActive { get; set; }
	public bool IsConfirmed { get; set; }

	public static UserDto Create(User user)
	{
		return new UserDto()
		{
			Id = user.Id,
			Email = user.Email,
			Name = user.Name,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Role = user.Role,
			Claims = user.Claims,
			IsActive = user.IsActive,
			IsConfirmed = user.EmailConfirm
		};
	}
}
