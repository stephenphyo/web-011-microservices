using AutoMapper;
using CommandService.DTOs;
using CommandService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("/api/c/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {

        /* Properties */
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;
        private readonly ICommandRepo _commandRepo;

        /* Constructor */
        public PlatformController(
            IMapper mapper,
            IPlatformRepo platformRepo,
            ICommandRepo commandRepo
        )
        {
            _mapper = mapper;
            _platformRepo = platformRepo;
            _commandRepo = commandRepo;
        }

        /* Methods */
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetAllPlatforms()
        {
            var platformItems = _platformRepo.GetAll();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("Inbound POST from Platform to Command");

            return Ok("Inbound Test from Platform to Command");
        }
    }
}