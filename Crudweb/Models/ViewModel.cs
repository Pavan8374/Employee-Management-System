using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CRUDWeb.Models
{
    public class ViewModel
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

        public virtual string? EmpSkills { get; set; }
        public string? Designation { get; set; }
    }
}
