using Microsoft.AspNetCore.Mvc;

namespace Tracksesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController: ControllerBase
    {

        public async Task<IActionResult> GetTotalSession()
        {
            return Ok();
        }
    }
}
