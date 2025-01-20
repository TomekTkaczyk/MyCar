﻿using MyCar.Module.Users.Core.Entities;

namespace MyCar.Module.Users.Core.Repositories;

internal interface IUserRepository
{

	Task<User> GetAsync(Guid id);

	Task<User> GetByEmailAsync(string email);

	Task<User> GetByNameAsync(string name);

	Task AddAsync(User user);

	Task UpdateAsync(User user);

	IQueryable<User> GetAll();

	Task DeleteAsync(User user);
}
