using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClinet;

        public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient,  IMessageBusClient  messageBusClinet)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClinet = messageBusClinet;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("==> Gettting Platforms");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }
        [HttpGet("{id}", Name ="GetPlatformById")]  
        public ActionResult<PlatformReadDto> GetPlatforms(int id)
        {
            var platformItems = _repository.GetPlatformById(id);
            if (platformItems != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItems));
            }else{
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            // Send Synchronous Message 
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"==>  Could not send synchronously: {ex.Message}");
            }

            // Send Asynchronous Message
            try
            {
              var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
              platformPublishedDto.Event = "Platform_Published";
              _messageBusClinet.PublishNewPlatform(platformPublishedDto);
            }
            catch (Exception ex)
            {
                
               Console.WriteLine($"==>  Could not send Aynchronously: {ex.Message}");
            }

           return Ok( _mapper.Map<PlatformReadDto>(platformModel));

        // var platformReadDto =  _mapper.Map<PlatformReadDto>(platformModel);
        // return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
        }
    }
}