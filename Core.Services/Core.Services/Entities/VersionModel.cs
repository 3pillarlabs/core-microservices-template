using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.Services.Entities
{
    public class VersionModel
    {
        public string Version { get; set; }
        [JsonProperty("Docs")]
        public List<VersionDocument> Documents { get; set; }
    }
}
