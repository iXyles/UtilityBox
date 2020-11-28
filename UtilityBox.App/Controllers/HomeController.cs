using Microsoft.AspNetCore.Mvc;

namespace UtilityBox.App.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("hello")]
        public IActionResult Index()
        {
            return StatusCode(200, new { message = "Hello World!" });
        }
    }
}