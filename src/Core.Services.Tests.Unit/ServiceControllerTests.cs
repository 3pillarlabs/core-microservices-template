using Castle.Core.Logging;
using Core.Services.Areas.V1.Controlllers;
using Core.Services.Areas.V1.Models.Requests;
using Core.Services.Areas.V1.Models.Responses;
using Core.Services.Configurations;
using Core.Services.Entities;
using Core.Services.Repositories.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Services.Tests.Unit
{
    [TestClass]
    public class ServiceControllerTests
    {
        #region Initialize 
        Mock<IAppSettings> mockAppSettings = null;       
        Mock<IDatabaseRepository> mockDatabaseRepository = null;
       

        [TestInitialize]
        public void Initialize()
        {
            mockAppSettings = new Mock<IAppSettings>();           
            mockDatabaseRepository = new Mock<IDatabaseRepository>();  
            mockDatabaseRepository.Setup(ee=>ee.AddEmployee(Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<int>(), Moq.It.IsAny<int>())).Returns(1);
            mockDatabaseRepository.Setup(ee => ee.RemoveEmployee(Moq.It.IsAny<int>())).Returns(1);
            mockDatabaseRepository.Setup(ee => ee.GetEmployeeDetailById(Moq.It.IsAny<int>())).Returns(new Entities.EmployeeDetail() { Name="test",Address="gg",Salary=333,DeptName="HR"});
            mockDatabaseRepository.Setup(ee => ee.GetEmployeesList()).Returns(new List<Employee>() { new Employee() {EmployeeId=101, Name = "test",  DeptName = "HR" } });
        }
        #endregion

        #region Validation Failed StatusCode: 400 
        [TestMethod]
        public void AddEmployee_400_Name_Missing()
        {
            var controller = new ServiceController(mockAppSettings.Object, mockDatabaseRepository.Object);
            var request = new AddEmployeeRequest() { };
            var validationResults = ValidateRequest(request);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            var response = controller.AddEmployee(request) as ObjectResult;
            var result = response.Value as AddEmployeeResponse;
            Assert.AreEqual(result.Success, false);
            Assert.IsTrue(result.ErrorResponse.Message.Contains("Name"));
            Assert.AreEqual(response.StatusCode, 400);
        }
        [TestMethod]
        public void AddEmployee_400_Address_Missing()
        {
            var controller = new ServiceController(mockAppSettings.Object, mockDatabaseRepository.Object);
            var request = new AddEmployeeRequest() {Name="Unit Testing",DepartmentId=1002,Salary=3200 };
            var validationResults = ValidateRequest(request);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            var response = controller.AddEmployee(request) as ObjectResult;
            var result = response.Value as AddEmployeeResponse;
            Assert.AreEqual(result.Success, false);
            Assert.IsTrue(result.ErrorResponse.Message.Contains("Address"));
            Assert.AreEqual(response.StatusCode, 400);
        }
        [TestMethod]
        public void AddEmployee_400_Salary_Missing()
        {
            var controller = new ServiceController(mockAppSettings.Object, mockDatabaseRepository.Object);
            var request = new AddEmployeeRequest() { Name = "Unit Testing", Address = "TestAddress", DepartmentId = 1002 };
            var validationResults = ValidateRequest(request);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            var response = controller.AddEmployee(request) as ObjectResult;
            var result = response.Value as AddEmployeeResponse;
            Assert.AreEqual(result.Success, false);
            Assert.IsTrue(result.ErrorResponse.Message.Contains("Salary"));
            Assert.AreEqual(response.StatusCode, 400);
        }
        [TestMethod]
        public void AddEmployee_200_OK()
        {
            var controller = new ServiceController(mockAppSettings.Object, mockDatabaseRepository.Object);
            var request = new AddEmployeeRequest() { Name = "Unit Testing", Address = "TestAddress", DepartmentId = 1002, Salary = 3200 };
            var validationResults = ValidateRequest(request);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
            var response = controller.AddEmployee(request) as ObjectResult;
            var result = response.Value as AddEmployeeResponse;
            Assert.AreEqual(result.Success, true);           
            Assert.AreEqual(response.StatusCode, 200);
        }
        [TestMethod]
        public void GetEmployeeById_200_OK()
        {
            var controller = new ServiceController(mockAppSettings.Object, mockDatabaseRepository.Object);           
            var response = controller.GetEmployeeById(101) as ObjectResult;
            var result = response.Value as GetEmployeeResponse;
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(response.StatusCode, 200);
        }
        [TestMethod]
        public void GetEmployeeList_200_OK()
        {
            var controller = new ServiceController(mockAppSettings.Object, mockDatabaseRepository.Object);
            var response = controller.GetEmployeeList() as ObjectResult;
            var result = response.Value as GetEmployeesListResponse;
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(response.StatusCode, 200);
        }
        [TestMethod]
        public void RemoveEmployee_200_OK()
        {
            var controller = new ServiceController(mockAppSettings.Object, mockDatabaseRepository.Object);
            var response = controller.RemoveEmployee(102) as ObjectResult;
            var result = response.Value as RemoveEmployeeResponse;
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(response.StatusCode, 200);
        }
        #endregion
        #region private methods
        private List<ValidationResult> ValidateRequest(AddEmployeeRequest request)
        {
            var validationContext = new ValidationContext(request, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, true);
            return validationResults;
        }
        #endregion
    }
}
