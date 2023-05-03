using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CRUD.Domain.Employee
{
    public class Emp
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "First Name is Required!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Alphabets are allowed!")]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Alphabets are allowed!")]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter valid Email address!")]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Gender is required!")]
        public string? Gender { get; set; }       
        public virtual List<SkillsList> Skills { get; set; }
        public string? Designation { get; set; }
        public string? ImageURL { get; set; }
    }
    public class SkillsList
    {
        public Guid Id { get; set; }
        public virtual Guid EmployeeId { get; set; }
        public string? Name { get; set; }
        public virtual Emp EmployeesTable { get; set; }
    }
}

