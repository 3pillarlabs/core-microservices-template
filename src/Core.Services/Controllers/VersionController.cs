using Core.Services.Entities;
using Core.Services.Filters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Core.Services.Controllers
{
    [Route("service")]
    public class VersionController : Controller
    {
        [Route("version")]
        [ServiceFilter(typeof(CustomExceptionFilter))]       
        [SwaggerResponse(200, typeof(VersionModel))]
        [HttpGet]
        public IActionResult GetVersion()
        {
            var projectionsAssembly = Assembly.GetEntryAssembly();
            var version = new VersionModel
            {
                Version = projectionsAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                Documents = new System.Collections.Generic.List<VersionDocument>
                {
                    new VersionDocument
                    {
                       Status = VersionStatus.Live.ToString()
                    }
                }
            };
            return Ok(version);
        }
    }
}
