using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Module.Users.Core.Services;
internal class IdentityService(
	IUserRepository userRepository,
	IPasswordHasher<User> passwordHasher,
	IAuthManager authManager,
	IClock clock) : IIdentityService
{
	private readonly IUserRepository _userRepository = userRepository;
	private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
	private readonly IAuthManager _authManager = authManager;
	private readonly IClock _clock = clock;

	public async Task<AccountDto> GetAsync(Guid id)
	{
		var user = await _userRepository.GetAsync(id);

		return user is null
			? null
			: new AccountDto
			{
				Id = user.Id,
				Email = user.Email,
				Role = user.Role,
				Claims = user.Claims,
				CreatedAt = user.CreatedAt
			};
	}

	public async Task<JsonWebToken> SignInAsync(SignInDto dto)
	{
		return new JsonWebToken();
	}

	public async Task SignUpAsync(SignUpDto dto)
	{
		var user = await _userRepository.GetAsync(dto.Email.ToLowerInvariant());
		if(user is not null) {
			throw new EmailIsInUseException();
		}

		var password = _passwordHasher.HashPassword(default, dto.Password);
		user = new User
		{
			Id = Guid.NewGuid(),
			Email = dto.Email,
			Password = password,
			Role = dto.Role?.ToLowerInvariant() ?? "user",
			CreatedAt = _clock.CurrentDate(),
			IsActive = true,
			Claims = dto.Claims ?? new Dictionary<string, IEnumerable<string>>()
		};

		await _userRepository.AddAsync(user);
	}

}
