using Microsoft.EntityFrameworkCore;
using MyCar.Module.Users.Core.Entities;
using MyCar.Module.Users.Core.Repositories;

namespace MyCar.Module.Users.Core.DAL.Repositories;
internal class UserRepository(UsersDbContext context) : IUserRepository
{
	private readonly UsersDbContext _context = context;
	private readonly DbSet<User> _users;

	public Task<User> GetAsync(Guid id)
		=> _users.SingleOrDefaultAsync(x => x.Id == id);

	public Task<User> GetAsync(string email)
		=> _users.SingleOrDefaultAsync(x => x.Email == email);

	public async Task AddAsync(User user)
	{
		await _users.AddAsync(user);
		await _context.SaveChangesAsync();
	}
	public async Task UpdateAsync(User user)
	{
		_users.Update(user);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(User user)
	{
		_users.Remove(user);
		await _context.SaveChangesAsync();
	}

	public async Task<IReadOnlyList<User>> GetAllAsync()
	{
		return await _users.AsNoTracking().ToListAsync();
	}

}
