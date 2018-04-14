using Core.Services.Entities;
using Core.Services.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Core.Services.Controllers
{
    [Route("service")]
    public class VersionController : Controller
    {
        [Route("version")]
        [ServiceFilter(typeof(CustomExceptionFilter))]
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
