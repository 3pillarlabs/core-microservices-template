using Core.Services.Areas.V1.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core.Services.Areas.V1.Controlllers
{  
    public partial class ServiceController : Controller
    {
        [Route("{Id:int}")]
        [HttpDelete]
        public IActionResult RemoveEmployee(int id)
        {
            var response =new RemoveEmployeeResponse();

            if(id<=0)
            {
                return BadRequest("Invalid Id..");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(e => e.Value.Errors.Count > 0).Select(ee => ee.Value.Errors.First().ErrorMessage);
               
                return BadRequest(errors);
            }

            var result = _dbRepository.RemoveEmployee(id);
            if (result > 0)
            {
                response.Result = true;
                response.Success = true;
            }
            return Ok(response);
                
        }
    }
 
}
