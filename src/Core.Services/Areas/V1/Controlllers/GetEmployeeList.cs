using Core.Services.Areas.V1.Models.Responses;
using Core.Services.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core.Services.Areas.V1.Controlllers
{  
    public partial class ServiceController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult GetEmployeeList()
        {
            var response =new GetEmployeesListResponse();

            var result=  _dbRepository.GetEmployeesList();            
            if (result != null && result.Any())
            {
                response.Result = result;
                response.Success = true;
            }
            else
            {
                response.ErrorResponse = Helpers.Helper.ConvertToErrorResponse("No employee record found..",ErrorsType.NoRecordFound.ToString() , ErrorMessageType.Validation.ToString());               
            }

            return Ok(response);
                
        }
    }
 
}
