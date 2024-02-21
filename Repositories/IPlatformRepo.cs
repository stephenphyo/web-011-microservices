using PlatformService.Models;

namespace PlatformService.Repositories
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAll();
        Platform? GetById(int id);
        void Create(Platform platform);
    }
}