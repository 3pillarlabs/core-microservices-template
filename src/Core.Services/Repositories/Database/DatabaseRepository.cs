using Core.Services.Configurations;
using Core.Services.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Services.Repositories.Database
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IAppSettings _appSettings;

        public DatabaseRepository(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        #region Private Methods
        private string ConnectionString
        {
            get
            {
                return _appSettings.ConnectionString;
            }
        }
        private void CreateProcedureCommand(SqlCommand command, string procedureName, SqlConnection connection, SqlParameter[] dbParameters = null)
        {

            command.CommandText = procedureName;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            if (dbParameters != null && dbParameters.Any())
                command.Parameters.AddRange(dbParameters);
        }

        #endregion

        #region Public Methods
        public EmployeeDetail GetEmployeeDetailById(int employeeId)
        {
            EmployeeDetail employee = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var sqlParameters = new List<SqlParameter>() { new SqlParameter("@empId", employeeId) };
                using (var command = new SqlCommand())
                {
                    CreateProcedureCommand(command, "usp_GetEmployeeDetail", connection, sqlParameters.ToArray());
                    using (var rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            employee = new EmployeeDetail();
                            employee.EmployeeId = Convert.ToInt32(rdr["EmployeeId"]);
                            employee.Name = Convert.ToString(rdr["Name"]);
                            employee.Address = Convert.ToString(rdr["Address"]);
                            employee.Salary = (!string.IsNullOrEmpty(Convert.ToString(rdr["Salary"]))) ? Convert.ToInt32(rdr["Salary"]) : 0;
                            employee.DeptName = Convert.ToString(rdr["DeptName"]);
                            employee.IsActive = Convert.ToBoolean(rdr["IsActive"]);
                        }
                    }
                }
                return employee;
            }
        }

        public List<Employee> GetEmployeesList()
        {
            var employeeList = new List<Employee>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                using (var command = new SqlCommand())
                {
                    CreateProcedureCommand(command, "usp_GetEmployeesList", connection, null);
                    using (var rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var employee = new Employee();
                            employee.EmployeeId = Convert.ToInt32(rdr["EMPLOYEEID"]);
                            employee.Name = Convert.ToString(rdr["NAME"]);                           
                            employee.DeptName = Convert.ToString(rdr["DeptName"]);
                            employeeList.Add(employee);
                        }
                    }
                }
                return employeeList;
            }
            #endregion
        }
        public int AddEmployee(string name, string address, int salary, int departmentId)
        {
            int result=0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var sqlParameters = new List<SqlParameter>() {
                    new SqlParameter("@name", name),
                    new SqlParameter("@address", address),
                    new SqlParameter("@salary", salary),                    
                    new SqlParameter("@deptId", departmentId)
                };
                using (var command = new SqlCommand())
                {
                    CreateProcedureCommand(command, "usp_AddEmployee", connection, sqlParameters.ToArray());
                    result = command.ExecuteNonQuery();
                }
                return result;
            }
        }
        public int RemoveEmployee(int employeeId)
        {
            int result = 0;
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var sqlParameters = new List<SqlParameter>() { new SqlParameter("@empId", employeeId) };
                
                using (var command = new SqlCommand())
                {
                    CreateProcedureCommand(command, "usp_DeleteEmployee", connection, sqlParameters.ToArray());
                    result = command.ExecuteNonQuery();
                }
                return result;
            }
        }
       
    }
}