
using Core.Services.Entities;
using System.Collections.Generic;

namespace Core.Services.Repositories.Database
{
    public interface IDatabaseRepository
    {
        List<Core.Services.Entities.Employee> GetEmployeesList();
        EmployeeDetail GetEmployeeDetailById(int employeeId);
        int AddEmployee(string name, string address, int salary, int departmentId);
        int RemoveEmployee(int employeeId);
    }
}
