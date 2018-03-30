IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'CoreServices')
Create Database CoreServices
GO

use CoreServices
GO

Create table Department(DepartmentId int primary key identity(1000,1) ,Name varchar(15))
GO

CREATE table Employee(EmployeeId int primary key identity(100,1),Name varchar(20),Address varchar(30),Salary int,IsActive bit,deptId int references Department)
GO


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
END

GO

INSERT INTO Department (Name) 
VALUES('DEVELOPMENT'),('HR'),('QA'),('SALES')

INSERT INTO Employee (Name,Address,Salary,IsActive,deptId) 
VALUES('Ram','Noida',3743,1,1003),
('Seeta','Gurgaon',5033,1,1000),
('Reeta','Delhi',2300,1,1001),
('Mohan','Noida',6522,1,1000)


