using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.Policies;

internal interface IUserDeletionPolicy
{
	Task<bool> CanDeleteAsync(User employee);
}
