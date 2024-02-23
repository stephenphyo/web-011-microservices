using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Repositories;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {
        /* Properties */
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusPublisher _messageBusPublisher;

        /* Constructor */
        public PlatformController(
            IPlatformRepo repo,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusPublisher messageBusPublisher)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusPublisher = messageBusPublisher;
        }

        /* Methods */
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetAllPlatforms()
        {
            var platformItems = _repo.GetAll();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platformItem = _repo.GetById(id);
            if (platformItem == null)
            {
                return NotFound();
            }
            Console.WriteLine(Environment.GetEnvironmentVariable("hello"));
            return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
        }

        [HttpPost(Name = "CreateNewPlatform")]
        public async Task<ActionResult<PlatformReadDTO>> CreateNewPlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDTO);
            _repo.Create(platformModel);
            _repo.SaveChanges();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(platformModel);

            /* Synchronous Message */
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Synchronous Messaging Failed; {e.Message}");
            }

            /* Asynchronous Message */
            try
            {
                var platformPublishedDTO = _mapper.Map<PlatformPublishedDTO>(platformReadDTO);
                platformPublishedDTO.Event = "Platform_Published";
                _messageBusPublisher.PublishNewPlatform(platformPublishedDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Asynchronous Messaging Failed; {e.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }
    }
}