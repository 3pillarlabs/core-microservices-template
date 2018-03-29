using Core.Services.Areas.V1.Models.Responses;
using Core.Services.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Core.Services.Areas.V1.Controlllers
{  
    public partial class ServiceController : Controller
    {
        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult RemoveEmployee(int id)
        {
            var response =new RemoveEmployeeResponse();

            if(id<=0)
            {
                response.ErrorResponse = Helpers.Helper.ConvertToErrorResponse("Invalid Id..", ErrorsType.ValidationError.ToString(), ErrorMessageType.Validation.ToString());
                return BadRequest(response);
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
            else
            {
                response.ErrorResponse = Helpers.Helper.ConvertToErrorResponse("Some error occured in removing employee info..", ErrorsType.DatabaseError.ToString(), ErrorMessageType.Error.ToString());
            }
            return Ok(response);
                
        }
    }
 
}
