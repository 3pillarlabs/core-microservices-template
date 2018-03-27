using Core.Services.Entities;

namespace Core.Services.Areas.V1.Models.Responses
{
    public class GetEmployeeResponse
    {
        public bool Success { get; set; }
        public EmployeeDetail Result { get; set; } 
        public ResponseMessage ErrorResponse { get; set; }
    }
}
