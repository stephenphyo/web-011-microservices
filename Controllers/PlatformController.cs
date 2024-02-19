using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("/api/c/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {

        /* Constructor */
        public PlatformController()
        {

        }

        /* Methods */
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("Inbound POST from Platform to Command");

            return Ok("Inbound Test from Platform to Command");
        }
    }
}