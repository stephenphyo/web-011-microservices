using CommandService.Models;

namespace CommandService.Repositories
{
    public interface IPlatformRepo
    {
        IEnumerable<Platform> GetAll();
        bool CheckPlatformExists(int platformId);
        bool CheckExternalPlatformExists(int extPlatformId);
        void Create(Platform platform);
        bool SaveChanges();
    }
}