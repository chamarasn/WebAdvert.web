using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.web.ServiceClients;

namespace WebAdvert.web.Controllers
{
    [Route("api")]
    [ApiController]
    [Produces("application/json")]
    public class InternalApis : Controller
    {
        private readonly IAdvertApiClient _advertApiClient;
        public InternalApis(IAdvertApiClient advertApiClient)
        {
            _advertApiClient = advertApiClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var record = await _advertApiClient.GetAsync(id).ConfigureAwait(false);
            return Json(record);
        }
    }
}
