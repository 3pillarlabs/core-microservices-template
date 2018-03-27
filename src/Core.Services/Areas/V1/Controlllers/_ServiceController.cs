using Core.Services.Configurations;
using Core.Services.Filters;
using Core.Services.Repositories.Database;
using Microsoft.AspNetCore.Mvc;

namespace Core.Services.Areas.V1.Controlllers
{
    [Route("/service/v1")]
    [ServiceFilter(typeof(CustomAuthorize))]
    public partial class ServiceController : Controller
    {
        public const int REFUND_NOTE_TYPE_ID = 24;
        public const int TAX_EXEMPT_ID = 2;
        private readonly IAppSettings _apiSettings;
        private readonly IDatabaseRepository _dbRepository;

        public ServiceController(IAppSettings apiSettings, IDatabaseRepository dbRepository)
        {
            _apiSettings = apiSettings;
            _dbRepository = dbRepository;
        }
    }
}
