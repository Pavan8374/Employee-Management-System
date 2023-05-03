using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Domain.Employee
{
    public interface IEmployeeService
    {
        Task<List<Emp>> GetAllEmployees();
        Task<Emp?> GetEmployeeById(Guid id);
        Task<Emp> AddEmployee(Emp emp);
        Task<Emp> EditEmployee(Emp emp);
        Task<Emp> DeleteEmployee(Emp emp);
        Task<List<SkillsList>> DeleteSkills(List<SkillsList> skills);
    }
}
