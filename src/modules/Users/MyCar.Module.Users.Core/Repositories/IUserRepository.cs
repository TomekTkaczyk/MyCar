using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.Repositories;

internal interface IUserRepository
{

	Task<User> GetAsync(Guid id);

	Task<User> GetAsync(string email);

	Task AddAsync(User user);

	Task UpdateAsync(User user);

	Task<IReadOnlyList<User>> GetAllAsync();

	Task DeleteAsync(User user);
}
