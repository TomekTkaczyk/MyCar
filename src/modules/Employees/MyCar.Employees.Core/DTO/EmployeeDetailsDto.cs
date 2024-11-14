using System.ComponentModel.DataAnnotations;

namespace MyCar.Employees.Core.DTO;
public class EmployeeDetailsDto : EmployeeDto
{
    [StringLength(1000)]
    public string Description { get; set; }
}
