
using System.ComponentModel.DataAnnotations;

namespace Core.Services.Areas.V1.Models.Requests
{
    public class AddEmployeeRequest
    {
        [Required(AllowEmptyStrings = false,ErrorMessage ="Name is required..")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required..")]
        public string Address { get; set; }
        [Range(1000,1100, ErrorMessage = "DepartmentId is required..")]        
        public int DepartmentId { get; set; }
        [Range(1000,200000,  ErrorMessage = "Salary is required..")]
        public int Salary { get; set; }
    }
}
