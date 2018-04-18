using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Core.Services.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeptId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                });

            migrationBuilder.Sql(InstallScript);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Employee");
            migrationBuilder.Sql(UninstallScript);
        }
        private const string InstallScript = @"
        CREATE PROC usp_GetEmployeesList
        AS
        BEGIN
        SELECT EMP.EMPLOYEEID,EMP.NAME,EMP.ADDRESS,DEPT.NAME  as DeptName FROM Employee EMP 
        INNER JOIN Department DEPT ON EMP.deptId=DEPT.DepartmentId
        Where EMP.IsActive=1
        END

        GO
        CREATE PROC usp_GetEmployeeDetail
        @empId int
        AS
        BEGIN
        SELECT EMP.EMPLOYEEID,EMP.NAME,EMP.ADDRESS,EMP.SALARY,EMP.IsActive,DEPT.NAME as DeptName FROM Employee EMP 
        INNER JOIN Department DEPT ON EMP.deptId=DEPT.DepartmentId
        WHERE EMP.EMPLOYEEID=@empId
        END

        GO
        CREATE PROC usp_AddEmployee
        @name varchar(20),
        @address varchar(30),
        @salary int,
        @deptId int
        AS
        BEGIN
        INSERT INTO Employee (Name,Address,Salary,isActive,deptId) 
        VALUES(@name,@address,@salary,1,@deptId)
        END

        GO
        CREATE PROC usp_DeleteEmployee
        @empId int
        AS
        BEGIN
        DELETE FROM Employee WHERE EMPLOYEEID=@empId
        END";

        private const string UninstallScript = @"
        DROP PROCEDURE [dbo].[usp_GetEmployeesList]
        GO
        DROP PROCEDURE [dbo].[usp_GetEmployeeDetail]
        GO
        DROP PROCEDURE [dbo].[usp_AddEmployee]
        GO
        DROP PROCEDURE [dbo].[usp_DeleteEmployee]
        GO";
    }
}
