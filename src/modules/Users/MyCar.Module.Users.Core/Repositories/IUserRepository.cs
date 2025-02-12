﻿using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.Repositories;

internal interface IUserRepository
{

	Task<User> GetAsync(Guid id, CancellationToken cancellationToken);

	Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);

	Task<User> GetByNameAsync(string name, CancellationToken cancellationToken);

	Task<User> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken);

	Task AddAsync(User user, CancellationToken cancellationToken);

	Task UpdateAsync(User user, CancellationToken cancellationToken);

	IQueryable<User> GetAll();

	Task DeleteAsync(User user, CancellationToken cancellationToken);
}
