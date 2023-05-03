using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Domain.Employee
{
    public class EmployeeService : IEmployeeService
    {
        public readonly IEmployeeRepository employeeRepository;
        public EmployeeService(IEmployeeRepository _employeeRepository)
        {
            employeeRepository = _employeeRepository;
        }
        public async Task<Emp> AddEmployee(Emp emp)
        {
            return await employeeRepository.AddEmployee(emp);
        }
        public async Task<Emp> DeleteEmployee(Emp emp)
        {
            return await employeeRepository.DeleteEmployee(emp);
        }
        public async Task<Emp> EditEmployee(Emp emp)
        {
            if(emp.Skills != null && emp.Skills.Count > 0)
            {
                var NewSkills = emp.Skills.Where(x => x.EmployeesTable == null).ToList();
                if(NewSkills != null && NewSkills.Count > 0)
                {
                    await employeeRepository.AddSkills(NewSkills);
                }
            }
            return await employeeRepository.EditEmployee(emp);
        }
        public async Task<List<Emp>> GetAllEmployees()
        {
            return await employeeRepository.GetAllEmployees();
        }
        public async Task<Emp?> GetEmployeeById(Guid id)
        {
            return await employeeRepository.GetEmployeeById(id);
        }
        public async Task<List<SkillsList>> DeleteSkills(List<SkillsList> skills)
        {
            return await employeeRepository.DeleteSkills(skills);
        }
    }
}
