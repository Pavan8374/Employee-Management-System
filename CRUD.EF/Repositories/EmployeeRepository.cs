
using CRUD.Domain.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.EF.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public readonly EmployeeDbContext dbContext;
        public EmployeeRepository(EmployeeDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<Emp>> GetAllEmployees()
        {
            return await dbContext.EmployeesTable.Include(e => e.Skills).AsNoTracking().ToListAsync();
        }
        public async Task<Emp?> GetEmployeeById(Guid id)
        {
            return await dbContext.EmployeesTable.AsNoTracking().Include(e => e.Skills).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Emp> AddEmployee(Emp emp)
        {
            emp.Id = Guid.NewGuid();
            dbContext.EmployeesTable.Add(emp);
            await dbContext.SaveChangesAsync();
            return emp;
        }
        public async Task<Emp> EditEmployee(Emp emp)
        {

            dbContext.EmployeesTable.Update(emp);
            await dbContext.SaveChangesAsync();
            return emp;
        }
        public async Task<Emp> DeleteEmployee(Emp emp)
        {
            dbContext.EmployeesTable.Remove(emp);
            await dbContext.SaveChangesAsync();
            return emp;
        }
        public async Task<List<SkillsList>> AddSkills(List<SkillsList> skills)
        {
            dbContext.Skills.AddRange(skills);
            await dbContext.SaveChangesAsync();
            return skills;
        }
        public async Task<List<SkillsList>> DeleteSkills(List<SkillsList> skills)
        {
            dbContext.Skills.RemoveRange(skills);
            await dbContext.SaveChangesAsync();
            return skills;
        }
    }
}
