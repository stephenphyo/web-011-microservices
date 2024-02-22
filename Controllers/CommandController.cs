using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using CommandService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("/api/c/Platform/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : Controller
    {
        /* Properties */
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;
        private readonly ICommandRepo _commandRepo;

        /* Constructor */
        public CommandController(
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
        public ActionResult<IEnumerable<CommandReadDTO>> GetCommandsForPlatform(int platformId)
        {
            if (!_platformRepo.CheckPlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItems = _commandRepo.GetAllCommandsForPlatform(platformId);

            return Ok(_mapper.Map<CommandReadDTO>(commandItems));
        }

        [HttpGet("{commandId}", Name = "GetCommandById")]
        public ActionResult<CommandReadDTO> GetCommandById(int platformId, int commandId)
        {

            if (_platformRepo.CheckPlatformExists(platformId)) return NotFound();

            var command = _commandRepo.GetById(platformId, commandId);
            if (command == null) return NotFound();

            return Ok(_mapper.Map<CommandReadDTO>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommandForPlatform(int platformId, CommandCreateDTO commandCreateDTO)
        {
            if (_platformRepo.CheckPlatformExists(platformId)) return NotFound();

            var command = _mapper.Map<Command>(commandCreateDTO);
            _commandRepo.Create(platformId, command);
            _commandRepo.SaveChanges();

            var commandReadDTO = _mapper.Map<CommandReadDTO>(command);

            return CreatedAtRoute(
                nameof(GetCommandById),
                new { platformId = platformId, commandId = commandReadDTO.Id },
                commandReadDTO
            );
        }
    }
}