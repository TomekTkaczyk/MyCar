using Microsoft.EntityFrameworkCore;
using MyCar.Module.Employees.Core.Entities;
using MyCar.Module.Employees.Core.Repositories;

namespace MyCar.Module.Employees.Core.DAL.Repositories;
internal class EmployeeRepository(EmployeesDbContext context) : IEmployeeRepository
{
	private readonly EmployeesDbContext _context = context;
	private readonly DbSet<Employee> _employees = context.Set<Employee>();

	public Task<Employee> GetAsync(Guid id) => _employees.SingleOrDefaultAsync(p => p.Id == id);

	public async Task<IReadOnlyList<Employee>> GetAllAsync() => await _employees.ToListAsync();

	public async Task AddAsync(Employee employee)
	{
		_employees.Add(employee);

		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(Employee employee)
	{
		_employees.Remove(employee);

		await _context.SaveChangesAsync();
	}


	public async Task UpdateAsync(Employee employee)
	{
		_employees.Update(employee);

		await _context.SaveChangesAsync();
	}
}
