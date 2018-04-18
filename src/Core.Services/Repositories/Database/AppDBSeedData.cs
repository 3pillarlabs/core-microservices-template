using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.Repositories.Database
{
    public class AppDBSeedData
    {
        AppDbContext _context;
        public AppDBSeedData(AppDbContext context)
        {
            _context = context;
        }
        public async Task EnsureSeeded()
        {
            if (!_context.Department.Any())
            {
                var deptList = new List<Department>() {
                new Department()
                {
                  Name="Dev"
                },
                new Department ()
                { Name="QA"}
                };

                _context.Department.AddRange(deptList);
                await _context.SaveChangesAsync();

            }
            if (!_context.Employee.Any())
            {
                var empList = new List<Employee>() {
                   new Employee()
                    {
                      DeptId=1,
                      Name="Ashok",
                      Address="Noida",
                      IsActive=true,
                      Salary=2993
                    },
                    new Employee()
                    {
                      DeptId=2,
                      Name="Reeta",
                      Address="Delhi",
                      IsActive=true,
                      Salary=3644
                    }
                };

                _context.Employee.AddRange(empList);
                await _context.SaveChangesAsync();
            }
        }
    }
}
