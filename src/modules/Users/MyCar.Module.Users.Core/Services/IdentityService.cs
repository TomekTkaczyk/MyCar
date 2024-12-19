using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Auth;
using MyCar.Shared.Infrastructure.Exceptions;
using MyCar.Shared.Infrastructure.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System.IdentityModel.Tokens.Jwt;

namespace MyCar.Module.Users.Core.Services;

internal class IdentityService(
	IUserRepository userRepository,
	IPasswordHasher<User> passwordHasher,
	ITokenProvider tokenProvider,
	IEmailConfirmerFactory emailConfirmerFactory,
	IClock clock) : IIdentityService
{
	public async Task<AccountDto> GetAsync(Guid id, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialsException();

		return user is null
			? null
			: new AccountDto
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = user.Role,
				Claims = user.Claims,
				IsConfirmed = user.EmailConfirm,
			};
	}

	public async Task<JsonWebToken> SignInAsync(SignInDto dto, CancellationToken cancellationToken)
	{
		var user = await GetByIdentifier(dto.Identifier.ToLowerInvariant())
			?? throw new InvalidCredentialsException();

		if(passwordHasher.VerifyHashedPassword(default, user.Password, dto.Password) is not PasswordVerificationResult.Success) {
			throw new InvalidCredentialsException();
		}

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		var jwt = GetTokens(user);

		await StoreRefreshToken(user, jwt.RefreshToken);

		return jwt;
	}

	public async Task<Guid> SignUpAsync(SignUpDto dto, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByEmailAsync(dto.Email.ToLowerInvariant());
		if(user is not null) {
			throw new EmailIsInUseException();
		}

		user = await userRepository.GetByNameAsync(dto.UserName.ToLowerInvariant());
		if(user is not null) {
			throw new NameIsInUseException();
		}

		var password = passwordHasher.HashPassword(default, dto.Password);

		user = new User
		{
			Id = Guid.NewGuid(),
			Name = dto.UserName.ToLowerInvariant(),
			Email = dto.Email.ToLowerInvariant(),
			Password = password,
			Role = "user",
			CreatedAt = clock.CurrentDate(),
			IsActive = true,
			EmailConfirm = false,
			Claims = []
		};

		await userRepository.AddAsync(user);

		// Create event e.g. UserCreated but in the meantime...
		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();
		if(emailConfirmer is not null) {
			var email = new Email
			{
				Body = emailConfirmer.GetConfirmEmailBody(user.Id, user.Email),
				Subject = "Activating your account in the MyCar application",
				Recievers = [user.Email]
			};
			EmailsQueue.Add(email);
			user.EmailConfirmToken = emailConfirmer.ConfirmToken;
		}
		else {
			user.EmailConfirm = true;
			user.EmailConfirmToken = null;
		}
		await userRepository.UpdateAsync(user);

		return user.Id;
	}

	public async Task<JsonWebToken> RefreshTokenAsync(string token, CancellationToken cancellationToken)
	{
		var handler = new JwtSecurityTokenHandler();
		var jwtToken = handler.ReadJwtToken(token);
		var sub = jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
		var exp = jwtToken.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
		if(exp != null && long.TryParse(exp, out var expUnixSecons)) {
		    var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnixSecons).UtcDateTime;
			var now = clock.CurrentDate();
			if(expirationDate < now) {
				throw new UnauthorisedException();
			}
		} else {
			throw new UnauthorisedException();
		};

		var user = await userRepository.GetAsync(new Guid(sub))
			?? throw new UnauthorisedException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		if(!user.RefreshToken.Equals(token)) {
			throw new UnauthorisedException();
		}

		var jwt = GetTokens(user);

		await StoreRefreshToken(user, jwt.RefreshToken);

		return jwt;
	}

	public async Task RemaindPasswordAsync(string email, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByEmailAsync(email)
			?? throw new InvalidCredentialsException();

		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();

		var forgotEmail = new Email
		{
			Body = emailConfirmer.GetRemaindPasswordBody(user.Id, user.Email),
			Subject = "Remaind your password in the MyCar application",
			Recievers = [user.Email]
		};
	}

	public async Task LogoutAsync(Guid id, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialsException();

		user.RefreshToken = null;

		await userRepository.UpdateAsync(user);
	}

	public async Task ChangePasswordAsync(Guid id, ChangePasswordDto dto, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialsException();

		if(passwordHasher.VerifyHashedPassword(default, user.Password, dto.CurrentPassword) is not PasswordVerificationResult.Success) {
			throw new InvalidPasswordException();
		}

		var password = passwordHasher.HashPassword(default, dto.Password);
		user.Password = password;

		await userRepository.UpdateAsync(user);
	}

	public async Task ChangeEmailAsync(Guid id, ChangeEmailDto dto, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialsException();

		var anotherUser = await userRepository.GetByEmailAsync(dto.Email);

		if(anotherUser is null) {
			var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();
			if(emailConfirmer is not null) {
				var email = new Email
				{
					Body = emailConfirmer.GetConfirmEmailBody(user.Id, user.Email),
					Subject = "Activating your account in the MyCar application",
					Recievers = [user.Email]
				};
				EmailsQueue.Add(email);
				user.EmailConfirmToken = emailConfirmer.ConfirmToken;
			}
			else {
				user.EmailConfirm = true;
				user.EmailConfirmToken = null;
			}
			return;
		}

		if(!user.Id.Equals(anotherUser.Id)) {
			throw new EmailIsInUseException();
		}
	}

	public async Task UpdateProfileAsync(Guid id, UpdateProfileDto dto, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialsException();

		user.FirstName = dto.FirstName;
		user.LastName = dto.LastName;

		await userRepository.UpdateAsync(user);
	}


	#region Private methods
	private async Task<User> GetByIdentifier(string identifier)
	{
		var user = await userRepository.GetByNameAsync(identifier);
		user ??= await userRepository.GetByEmailAsync(identifier);

		return user;
	}

	private JsonWebToken GetTokens(User user)
	{
		var jwt = new JsonWebToken
		{
			AccessToken = tokenProvider.GenerateAccessToken(user.Id, user.Role, user.Claims),
			RefreshToken = tokenProvider.GenerateRefreshToken(user.Id),
		};

		return jwt;
	}

	private async Task StoreRefreshToken(User user, string token)
	{
		user.RefreshToken = token;
		await userRepository.UpdateAsync(user);
	}

	#endregion
}
