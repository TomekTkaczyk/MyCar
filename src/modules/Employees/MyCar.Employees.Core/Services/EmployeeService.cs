using MyCar.Employees.Core.DTO;
using MyCar.Employees.Core.Entities;
using MyCar.Employees.Core.Exceptions;
using MyCar.Employees.Core.Policies;
using MyCar.Employees.Core.Repositories;
using MyCar.Employees.Core.ValueObjects;

namespace MyCar.Employees.Core.Services;

internal class EmployeeService(
	IEmployeeRepository employeeRepository,
	IEmployeeDeletionPolicy employeeDeletionPolicy ) : IEmployeeService
{
	private readonly IEmployeeRepository _employeeRepository = employeeRepository;
	private readonly IEmployeeDeletionPolicy _employeeDeletionPolicy = employeeDeletionPolicy;

	public async Task AddAsync(EmployeeDto dto) {
		dto.Id = Guid.NewGuid();
		await _employeeRepository.AddAsync(new Employee {
			Id = dto.Id,
			Firstname = dto.Firstname,
			Lastname = dto.Lastname
		});
	}

	public async Task<EmployeeDetailsDto> GetAsync(Guid id) {
		var employee = await _employeeRepository.GetAsync(id) ?? throw new EmployeeNotFoundException(id);

		var dto = Map<EmployeeDetailsDto>(employee);
		dto.Description = employee.Description;

		return dto;
	}

	public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync() {

		var employees = await _employeeRepository.GetAllAsync();

		return employees.Select(Map<EmployeeDto>).ToList();
	}

	public async Task UpdateAsync(EmployeeDetailsDto dto) {

		var employee = await _employeeRepository.GetAsync(dto.Id) ?? throw new EmployeeNotFoundException(dto.Id);
		
		employee.Firstname = dto.Firstname;
		employee.Lastname = dto.Lastname;
		employee.Description = dto.Description;

		await _employeeRepository.UpdateAsync(employee);
	}

	public async Task DeleteAsync(Guid id) {
		var employee = await _employeeRepository.GetAsync(id) ?? throw new EmployeeNotFoundException(id);

		if(await _employeeDeletionPolicy.CanDeleteAsync(employee) is false) {
			throw new CannotDeleteEmployeeException(id);
		}

		await _employeeRepository.DeleteAsync(employee);
	}

	private static T Map<T>(Employee employee) where T : EmployeeDto, new() {
		return new T() {
			Id = employee.Id,
			Firstname = employee.Firstname,
			Lastname = employee.Lastname,
		};
	}
}
