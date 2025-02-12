using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyCar.Shared.Abstractions.Auth;
using MyCar.Shared.Abstractions.Modules;
using MyCar.Shared.Abstractions.Services;
using MyCar.Shared.Infrastructure.Middleware;
using System.Text;

namespace MyCar.Shared.Infrastructure.Auth;
public static class Extensions
{
	public static IServiceCollection AddAuth(
		this IServiceCollection services,
		IConfiguration configuration,
		IList<IModule> modules,
		Action<JwtBearerOptions> optionsFactory = null)
	{
		var authOptions = configuration.GetOptions<AuthOptions>(AuthOptions.Section);

		services.AddSingleton<ITokenProvider, TokenProvider>();
		services.AddSingleton<IEmailConfirmerFactory, EmailConfirmerFactory>();

		var tokenValidationParameters = new TokenValidationParameters
		{
			RequireAudience = authOptions.RequireAudience,
			ValidIssuer = authOptions.ValidIssuer,
			ValidIssuers = authOptions.ValidIssuers,
			ValidateActor = authOptions.ValidateActor,
			ValidAudience = authOptions.ValidAudience,
			ValidAudiences = authOptions.ValidAudiences,
			ValidateAudience = authOptions.ValidateAudience,
			ValidateIssuer = authOptions.ValidateIssuer,
			ValidateLifetime = authOptions.ValidateLifetime,
			ValidateTokenReplay = authOptions.ValidateTokenReplay,
			ValidateIssuerSigningKey = authOptions.ValidateIssuerSigningKey,
			SaveSigninToken = authOptions.SaveSigninToken,
			RequireExpirationTime = authOptions.RequireExpirationTime,
			RequireSignedTokens = authOptions.RequireSignedTokens,
			ClockSkew = TimeSpan.Zero
		};

		if(string.IsNullOrWhiteSpace(authOptions.IssuerSigningKey)) {
			throw new ArgumentException("Missing IssuerSigningKey in options.", nameof(authOptions.IssuerSigningKey));
		}

		if(!string.IsNullOrWhiteSpace(authOptions.AuthenticationType)) {
			tokenValidationParameters.AuthenticationType = authOptions.AuthenticationType;
		}

		tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(authOptions.IssuerSigningKey));

		if(!string.IsNullOrWhiteSpace(authOptions.NameClaimType)) {
			tokenValidationParameters.NameClaimType = authOptions.NameClaimType;
		}

		if(!string.IsNullOrWhiteSpace(authOptions.RoleClaimType)) {
			tokenValidationParameters.RoleClaimType = authOptions.RoleClaimType;
		}

		services.AddAuthentication(o =>
			{
				o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(o =>
			{
				o.Authority = authOptions.Authority;
				o.Audience = authOptions.Audience;
				o.MetadataAddress = authOptions.MetadataAddress;
				o.SaveToken = authOptions.SaveToken;
				o.RefreshOnIssuerKeyNotFound = authOptions.RefreshOnIssuerKeyNotFound;
				o.RequireHttpsMetadata = authOptions.RequireHttpsMetadata;
				o.IncludeErrorDetails = authOptions.IncludeErrorDetails;
				o.TokenValidationParameters = tokenValidationParameters;
				if(!string.IsNullOrWhiteSpace(authOptions.Challenge)) {
					o.Challenge = authOptions.Challenge;
				}

				// add cookies
				o.Events = new JwtBearerEvents
				{
					OnMessageReceived = (context) =>
					{
						var cookieToken = context.Request.Cookies["accessToken"];
						if(!string.IsNullOrEmpty(cookieToken)) {

							context.Token = cookieToken;
						};
						return Task.CompletedTask;
					}
				};

				optionsFactory?.Invoke(o);
			});

		services.AddSingleton(authOptions);
		services.AddSingleton(tokenValidationParameters);

		services.AddAuthorization(auth =>
		{
			foreach(var module in modules) {
				foreach(var policy in module.Policies) {
					var policyName = $"{module.Name}.{policy}";
					auth.AddPolicy(policyName, policy => policy.RequireClaim("permissions", policyName));
				}
			}
		});

		return services;
	}
}
