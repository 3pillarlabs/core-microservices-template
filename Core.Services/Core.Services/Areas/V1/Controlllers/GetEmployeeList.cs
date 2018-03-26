using Core.Services.Areas.V1.Models.Responses;
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
            return Ok(response);
                
        }
    }
 
}
