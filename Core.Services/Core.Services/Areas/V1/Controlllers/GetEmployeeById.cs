using Core.Services.Areas.V1.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core.Services.Areas.V1.Controlllers
{  
    public partial class ServiceController : Controller
    {
        [Route("{id:int}")]
        [HttpGet]
        public IActionResult GetEmployeeById(int id)
        {
            var response =new GetEmployeeResponse();

            if(id<=0)
            {
                return BadRequest("Invalid Id..");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(e => e.Value.Errors.Count > 0).Select(ee => ee.Value.Errors.First().ErrorMessage);
               
                return BadRequest(errors);
            }

            var result = _dbRepository.GetEmployeeDetailById(id);
            if (result != null)
            {
                response.Result = result;
                response.Success = true;
            }
            return Ok(response);
                
        }
    }
 
}
