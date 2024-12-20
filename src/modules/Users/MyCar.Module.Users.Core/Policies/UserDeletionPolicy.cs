using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.Policies;
internal class UserDeletionPolicy : IUserDeletionPolicy
{
	public async Task<bool> CanDeleteAsync(User employee)
	{
		await Task.CompletedTask;

		return true;
	}
}
