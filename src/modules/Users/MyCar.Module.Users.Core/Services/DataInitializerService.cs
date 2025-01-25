using Microsoft.AspNetCore.Identity;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.Core.Services;
internal class DataInitializerService(IUserRepository repository, IPasswordHasher<User> passwordHasher) : IDataInitializerService
{
	public async Task Initialize()
	{
		var user = await repository.GetByNameAsync("admin", default);
		if(user == null) {
			await AddAdmin();
		}
	}

	private async Task  AddAdmin()
	{
		var user = new User
		{
			Id = Guid.NewGuid(),
			Name = "admin",
			Password = passwordHasher.HashPassword(default, ""),
			Role = "admin",
			IsActive = true,
			EmailConfirm = true,
		};

		await repository.AddAsync(user, default);
	}
}
