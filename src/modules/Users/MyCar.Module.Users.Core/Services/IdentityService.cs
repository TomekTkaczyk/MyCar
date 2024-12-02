using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Auth;
using MyCar.Shared.Infrastructure.Services;

namespace MyCar.Module.Users.Core.Services;
internal class IdentityService(
	IUserRepository userRepository,
	IPasswordHasher<User> passwordHasher,
	ITokenProvider tokenProvider,
	EmailConfirmerFactory emailConfirmerFactory,
	IClock clock) : IIdentityService
{
	public async Task<AccountDto> GetAsync(Guid id, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id);

		return user is null
			? null
			: new AccountDto
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				Role = user.Role,
				Claims = user.Claims,
				CreatedAt = user.CreatedAt
			};
	}

	public async Task<JsonWebToken> SignInAsync(SignInDto dto, CancellationToken cancellationToken)
	{
		var user = await GetByIdentifier(dto.Identifier.ToLowerInvariant())
			?? throw new InvalidCredentialException();

		if(passwordHasher.VerifyHashedPassword(default, user.Password, dto.Password) is not PasswordVerificationResult.Success) {
			throw new InvalidCredentialException();
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

		user = await userRepository.GetByNameAsync(dto.Name.ToLowerInvariant());
		if(user is not null) {
			throw new NameIsInUseException();
		}

		var password = passwordHasher.HashPassword(default, dto.Password);

		user = new User
		{
			Id = Guid.NewGuid(),
			Name = dto.Name.ToLowerInvariant(),
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

	public async Task<JsonWebToken> RefreshTokenAsync(Guid id, string token, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		if(!user.RefreshToken.Equals(token) || (user.Expires < clock.CurrentDate())) {
			throw new InvalidCredentialException();
		}

		var jwt = GetTokens(user);

		await StoreRefreshToken(user, jwt.RefreshToken);

		return jwt;
	}

	private JsonWebToken GetTokens(User user)
	{
		var jwt = new JsonWebToken
		{
			AccesToken = tokenProvider.GenerateAccessToken(user.Id, user.Role, user.Claims),
			RefreshToken = tokenProvider.GenerateRefreshToken(),
		};

		return jwt;
	}

	private async Task StoreRefreshToken(User user, RefreshToken token)
	{
		user.RefreshToken = token.Token;
		user.Expires = token.Expires;

		await userRepository.UpdateAsync(user);
	}

	public async Task ConfirmEmail(ConfirmEmailDto dto, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByEmailAsync(dto.Email)
			?? throw new InvalidCredentialException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();
		if(!emailConfirmer.Confirm(user.EmailConfirmToken, dto.ConfirmToken)) {
			throw new UserEmailConfirmException();
		}
	}

	public Task ForgotPassword(string email, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public async Task Logout(Guid id, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id);

		if(user is not null) {
			user.RefreshToken = null;
		};

		await userRepository.UpdateAsync(user);
	}

	private async Task<User> GetByIdentifier(string identifier)
	{
		var user = await userRepository.GetByNameAsync(identifier);
		user ??= await userRepository.GetByEmailAsync(identifier);

		return user;
	}
}
