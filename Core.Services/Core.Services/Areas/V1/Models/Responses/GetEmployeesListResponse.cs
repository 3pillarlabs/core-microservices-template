using Core.Services.Entities;
using System.Collections.Generic;

namespace Core.Services.Areas.V1.Models.Responses
{
    public class GetEmployeesListResponse
    {
        public bool Success { get; set; }
        public List<Employee> Result { get; set; }
        public ResponseMessage ErrorResponse { get; set; }
    }
}
