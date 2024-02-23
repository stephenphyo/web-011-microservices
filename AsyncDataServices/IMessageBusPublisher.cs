using PlatformService.DTOs;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusPublisher
    {
        void PublishNewPlatform(PlatformPublishedDTO platform);
    }
}