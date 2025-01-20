using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCar.Module.Users.Core.DAL;
using MyCar.Module.Users.Core.DAL.Repositories;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Repositories;
using MyCar.Module.Users.Core.Services;
using MyCar.Shared.Infrastructure.Database;

namespace MyCar.Module.Users.Core;

internal static class Extensions
{
	public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
		services.AddTransient<IIdentityService, IdentityService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IDataInitializerService, DataInitializerService>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IEmailVerificationService, EmailVerificationService>();
		services.AddDatatabase<UsersDbContext>(configuration);

		return services;
	}
}
