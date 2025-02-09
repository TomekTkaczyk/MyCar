﻿using MyCar.Module.Users.Core.DTO;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Module.Users.Core.Services;

public interface IIdentityService
{
	Task<AccountDto> GetAsync(Guid id, CancellationToken cancellationToken);
	Task<JsonWebToken> SignInAsync(SignInDto dto, CancellationToken cancellationToken);
	Task<JsonWebToken> RefreshTokenAsync(string token, CancellationToken cancellationToken);
	Task<Guid> SignUpAsync(SignUpDto dto, string confirmEmailUrl, CancellationToken cancellationToken);
	Task RemaindPasswordAsync(string email, string frontendConfirmEmailUrl, CancellationToken cancellationToken);
	Task LogoutAsync(Guid id, CancellationToken cancellationToken);
	Task ChangePasswordAsync(Guid id, ChangePasswordDto dto, CancellationToken cancellationToken);
	Task ChangeEmailAsync(Guid id, string email, string frontendConfirmEmailUrl, CancellationToken cancellationToken);
	Task UpdateProfileAsync(Guid id, UserProfileDto dto, CancellationToken cancellationToken);
	Task ConfirmEmailAsync(string token, CancellationToken cancellationToken);
	Task<string[]> GetAllPermissionsAsync(CancellationToken cancellationToken);
}