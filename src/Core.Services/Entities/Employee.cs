
namespace Core.Services.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }          
        public string DeptName { get; set; } 
    }
    public class EmployeeDetail : Employee
    {
        public string Address { get; set; }
        public int Salary { get; set; }
        public bool IsActive { get; set; }
    }
}
