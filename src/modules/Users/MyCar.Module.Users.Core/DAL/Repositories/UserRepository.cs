using MyCar.Module.Users.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCar.Module.Users.Core.DAL.Repositories;
internal class UserRepository(UsersDbContext context) : IUserRepository
{
	private readonly UsersDbContext _context = context;
}
