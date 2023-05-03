using AutoMapper;
using CRUD.Domain.Employee;
using CRUD.EF;
using CRUDWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using X.PagedList;

namespace CRUDWeb.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext context;
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        public EmployeeController(IEmployeeService _employeeService, EmployeeDbContext _context, IMapper _mapper, IWebHostEnvironment _env)
        {
            employeeService = _employeeService;
            webHostEnvironment = _env;
            context = _context;
            mapper = _mapper;

        }
        public async Task<IActionResult> Index(int? page, string searchString, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;
            var employees = await employeeService.GetAllEmployees();
            var emp1 = from s in employees select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                emp1 = emp1.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "firstname_desc":
                    emp1 = emp1.OrderByDescending(e => e.FirstName);
                    break;
                default:
                    emp1 = emp1.OrderBy(e => e.FirstName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var emp2 = emp1.ToPagedList(pageNumber, pageSize);
            var emp3 = emp2.ToList();

            return View(emp2);
        }
        public async Task<IActionResult> Post(EmpRequestModel empview)
        {
            var DTOmapped = mapper.Map<Emp>(empview);
            return Ok(DTOmapped);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EmpRequestModel emp)
        {
            if (ModelState.IsValid)
            {
                var employee = mapper.Map<Emp>(emp);
                if (emp.Image != null)
                {
                    string folder = "images/";
                    folder += Guid.NewGuid().ToString() + "_" + emp.Image.FileName;
                    string serverfolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                    await emp.Image.CopyToAsync(new FileStream(serverfolder, FileMode.Create));
                    employee.ImageURL = "/" + folder;

                }
                if (!string.IsNullOrEmpty(emp.EmpSkills))
                {
                    employee.Skills = new List<SkillsList>();
                    var skills = emp.EmpSkills.Trim().Split(" ").ToList();
                    foreach (var item in skills)
                    {
                        SkillsList skill = new SkillsList();
                        skill.Id = Guid.NewGuid();
                        skill.Name = item;
                        employee.Skills.Add(skill);
                    }
                }                
                await employeeService.AddEmployee(employee);
                TempData["Success"] = "Employee Registered Successfully!";
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var emp = await employeeService.GetEmployeeById(id.Value);
            if (emp == null)
            {
                return NotFound();
            }
            var emp1 = mapper.Map<EmpRequestModel>(emp);
            return View(emp1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EmpRequestModel emp)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = await employeeService.GetEmployeeById(id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }
                mapper.Map(emp, existingEmployee);
                List<string> skills = null;
                if (!string.IsNullOrEmpty(emp.EmpSkills))
                {
                    skills = emp.EmpSkills.Split(",").ToList();
                    if (skills != null && skills.Count > 0)
                    {
                        var deletedSkill = existingEmployee.Skills.Where(x => !skills.Contains(x.Name)).ToList();
                        if (deletedSkill != null && deletedSkill.Count > 0)
                        {
                            await employeeService.DeleteSkills(deletedSkill);
                        }
                    }
                    foreach (var skillName in skills)
                    {
                        if (!existingEmployee.Skills.Any(x => x.Name == skillName))
                        {
                            existingEmployee.Skills.Add(new SkillsList
                            {
                                Id = Guid.NewGuid(),
                                Name = skillName,
                                EmployeeId = existingEmployee.Id

                            });
                        }
                    }
                }
                if (emp.Image != null)
                {
                    string folder = "images/";
                    folder += Guid.NewGuid().ToString() + "_" + emp.Image.FileName;
                    string serverfolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                    using (var fileStream = new FileStream(serverfolder, FileMode.Create))
                    {
                        await emp.Image.CopyToAsync(fileStream);
                    }
                    existingEmployee.ImageURL = "/" + folder;
                }
                await employeeService.EditEmployee(existingEmployee);
                TempData["Success"] = "Employee Record Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var emp = await employeeService.GetEmployeeById(id.Value);
            if (emp == null)
            {
                return NotFound();
            }
            var emp1 = mapper.Map<EmpRequestModel>(emp);
            return View(emp1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Emp emp)
        {
            await employeeService.DeleteEmployee(emp);

            TempData["Success"] = "Employee Record Deleted Successfully!";
            return RedirectToAction("Index");

            return View(emp);
        }
    }
}
