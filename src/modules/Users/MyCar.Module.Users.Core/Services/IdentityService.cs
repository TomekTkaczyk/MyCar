using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.DTO;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Exceptions;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Shared.Abstractions;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Exceptions;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Exceptions;
using MyCar.Shared.Infrastructure.Services;
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
				IsConfirmed = user.EmailConfirm,
				Role = user.Role,
				Claims = user.Claims
			};
	}

	public async Task<JsonWebToken> SignInAsync(SignInDto dto, CancellationToken cancellationToken)
	{
		var user = await GetByIdentifier(dto.Identifier.ToLowerInvariant())
			?? throw new InvalidCredentialsException();

		if(passwordHasher.VerifyHashedPassword(default, user.Password, dto.Password) is not PasswordVerificationResult.Success) {
			throw new InvalidCredentialsException();
		}

		var jwt = GetTokens(user);

		await StoreRefreshToken(user, jwt.RefreshToken);

		return jwt;
	}

	public async Task<Guid> SignUpAsync(SignUpDto dto, string frontendConfirmEmailUrl, CancellationToken cancellationToken)
	{
		var error = new ApiError();
		var user = await userRepository.GetByEmailAsync(dto.Email.ToLowerInvariant());
		if(user is not null) {
			error.AddValidationError("Email", "email_is_unavailable", "Email is unavailable.");
		}

		user = await userRepository.GetByNameAsync(dto.UserName.ToLowerInvariant());
		if(user is not null) {
			error.AddValidationError("UserName", "username_is_unavailable", "UserName is unavailable.");
		}

		if(error.ValidationErrors.Any()) {
			throw new InvalidCredentialsException();
		}

		var password = passwordHasher.HashPassword(default, dto.Password);

		user = new User
		{
			Id = Guid.NewGuid(),
			Name = dto.UserName.ToLowerInvariant(),
			Email = dto.Email.ToLowerInvariant(),
			Password = password,
			Role = "user",
			Claims = null,
			CreatedAt = clock.CurrentDate(),
			IsActive = true,
			EmailConfirm = false,
		};

		await userRepository.AddAsync(user);

		var token = tokenProvider.GenerateConfirmEmailToken(user.Id, user.Email);

		user.EmailConfirmToken = token;

		await CreateEmail(
			frontendConfirmEmailUrl, 
			dto.Email,
			token,
			cancellationToken);

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

	public async Task RemaindPasswordAsync(string email, string frontendConfirmEmailUrl, CancellationToken cancellationToken)
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

		EmailsQueue.Add(forgotEmail);
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

	public async Task ChangeEmailAsync(Guid id, string emailAddress, string frontendConfirmEmailUrl, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialsException();

		var anotherUser = await userRepository.GetByEmailAsync(emailAddress);

		if(anotherUser is not null) {
			if(!user.Id.Equals(anotherUser.Id)) {
				throw new EmailIsInUseException();
			}
			throw new EmailNotChanged();
		}

		var token = tokenProvider.GenerateConfirmEmailToken(id, emailAddress);

		user.EmailConfirmToken = token;

		await CreateEmail(
			frontendConfirmEmailUrl,
			emailAddress,
			token,
			cancellationToken);

		await userRepository.UpdateAsync(user);
	}

	public async Task UpdateProfileAsync(Guid id, UserProfileDto dto, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetAsync(id)
			?? throw new InvalidCredentialsException();

		user.FirstName = dto.FirstName;
		user.LastName = dto.LastName;

		await userRepository.UpdateAsync(user);
	}

	public async Task ConfirmEmailAsync(string token, CancellationToken cancellationToken)
	{

		var confirmToken = DecodeJwt(token);
		var idClaim = confirmToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
			?? throw new InvalidEmailTokenException();

		if(!Guid.TryParse(idClaim, out Guid id)) {
			throw new InvalidEmailTokenException();
		}

		var emailClaim = confirmToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value
			?? throw new InvalidEmailTokenException();

		var user = await userRepository.GetAsync(id)
			?? throw new InvalidEmailTokenException();

		if(!user.IsActive) {
			throw new UserNotActiveException(user.Id);
		}

		var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer(EmailConfirmTypes.Jwt);
		if(!emailConfirmer.Confirm(user.EmailConfirmToken, token, emailClaim)) {
			throw new UserEmailConfirmException();
		}

		user.Email = emailClaim;
		user.EmailConfirm = true;
		user.EmailConfirmToken = null;

		await userRepository.UpdateAsync(user);
	}

	public async Task ResendConfirmEmailTokenAsync(string email, string frontendConfirmEmailUrl, CancellationToken cancellationToken)
	{
		await Task.CompletedTask;
		throw new NotImplementedException();

		//var user = await userRepository.GetByEmailAsync(email)
		//	?? throw new UserEmailConfirmException();

		//if(!user.IsActive) {
		//	throw new UserNotActiveException(user.Id);
		//}

		//var emailConfirmer = emailConfirmerFactory.GetEmailConfirmer();
		//if(emailConfirmer is not null) {
		//	await CreateEmail(frontendConfirmEmailUrl, email, cancellationToken);
		//}
		//else {
		//	user.EmailConfirm = true;
		//	user.EmailConfirmToken = null;
		//}
		//await userRepository.UpdateAsync(user);
	}

	public async Task<string[]> GetAllPermissionsAsync(CancellationToken cancellationToken)
	{
		await Task.CompletedTask;
		throw new NotImplementedException();
	}


	#region Private methods
	private static async Task CreateEmail(string frontendConfirmEmailUrl, string emailAddress, string token, CancellationToken cancellationToken)
	{
		var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "EmailConfirmTokenTemplate.html");
		var template = await File.ReadAllTextAsync(path, cancellationToken);
		var confirmUrl = $"{frontendConfirmEmailUrl}?ConfirmToken={token}";
		var email = new Email
		{
			Body = template.Replace("{{ConfirmUrl}}", confirmUrl),
			Subject = "Potwierdzenie adresu email w aplikacji MyCar",
			Recievers = [emailAddress]
		};
		EmailsQueue.Add(email);
	}

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

	private static JwtSecurityToken DecodeJwt(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		if(!tokenHandler.CanReadToken(token)) {
			throw new InvalidEmailTokenException();
		}

		return tokenHandler.ReadJwtToken(token);
	}


	#endregion
}
