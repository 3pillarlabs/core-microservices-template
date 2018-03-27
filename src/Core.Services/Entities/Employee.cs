
namespace Core.Services.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }        
        public string DeptName { get; set; }     
        public int Salary { get; set; }
        public bool IsActive { get; set; }

    }
}
