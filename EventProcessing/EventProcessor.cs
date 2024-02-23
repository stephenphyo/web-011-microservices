using System.Text.Json;
using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using CommandService.Repositories;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        /* Properties */
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;

        /* Constructor */
        public EventProcessor(
            IServiceScopeFactory scopeFactory,
            IMapper mapper
        )
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        /* Methods */
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    Console.WriteLine("---Platform Published Event Detected---");
                    addPlatform(message);
                    break;
                default:
                    break;
            }
        }

        /* Private Methods */
        private void addPlatform(string incomingMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var commandRepo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformRepo = scope.ServiceProvider.GetRequiredService<IPlatformRepo>();

                var platformPublishedDTO = JsonSerializer.Deserialize<PlatformPublishedDTO>(incomingMessage);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDTO);
                    Console.WriteLine(platform.Id);
                    Console.WriteLine(platform.ExternalId);
                    if (!platformRepo.CheckExternalPlatformExists(platform.ExternalId))
                    {
                        platformRepo.Create(platform);
                        platformRepo.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Platform Already Existed");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private EventType DetermineEvent(string incomingMessage)
        {
            Console.WriteLine("---Determining Event Type---");
            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(incomingMessage);

            switch (eventType?.Event)
            {
                case "Platform_Published":
                    return EventType.PlatformPublished;
                default:
                    return EventType.Undetermined;
            }
        }

        enum EventType
        {
            PlatformPublished,
            Undetermined
        }
    }
}