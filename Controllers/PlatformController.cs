using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : Controller
    {

        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        public PlatformController(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

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
            return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
        }

        [HttpPost(Name = "CreateNewPlatform")]
        public ActionResult<PlatformReadDTO> CreateNewPlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDTO);
            _repo.Create(platformModel);
            _repo.SaveChanges();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDTO.Id }, platformReadDTO);
        }
    }
}