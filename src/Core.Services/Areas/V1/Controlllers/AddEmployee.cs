using Core.Services.Areas.V1.Models.Requests;
using Core.Services.Areas.V1.Models.Responses;
using Core.Services.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core.Services.Areas.V1.Controlllers
{  
    public partial class ServiceController : Controller
    {
        [Route("")]
        [HttpPost]
        public IActionResult AddEmployee([FromBody]AddEmployeeRequest request)
        {
            var response =new AddEmployeeResponse();
            
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(e => e.Value.Errors.Count > 0).Select(ee => ee.Value.Errors.First().ErrorMessage);
                response.ErrorResponse = Helpers.Helper.ConvertToErrorResponse(errors.FirstOrDefault(), ErrorsType.ValidationError.ToString(), ErrorMessageType.Validation.ToString());
                return BadRequest(response);               
            }

            var result = _dbRepository.AddEmployee(request.Name,request.Address,request.Salary,request.DepartmentId);
            if (result > 0)
            {
                response.Result = true;
                response.Success = true;
            }
            else
            {
                response.ErrorResponse = Helpers.Helper.ConvertToErrorResponse("Some error occured in adding employee..", ErrorsType.DatabaseError.ToString(), ErrorMessageType.Error.ToString());
            }
            return Ok(response);
                
        }
    }
 
}
