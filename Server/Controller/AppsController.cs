using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UtilityBox.App.Server.Services;

namespace UtilityBox.App.Server.Controller
{
    [Route("api/apps")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private readonly WindowsAppService _appService;

        public AppsController(WindowsAppService appService)
        {
            _appService = appService;
        }


        [HttpGet("installed")]
        public async Task<JsonResult> GetApps() =>
            new JsonResult(await _appService.CheckInstalledApps());

    }
}
