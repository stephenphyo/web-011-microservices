using PlatformService.Models;

namespace PlatformService.Repositories
{
    public interface IPlatformRepo
    {
        IEnumerable<Platform> GetAll();
        Platform? GetById(int id);
        void Create(Platform platform);
        bool SaveChanges();
    }
}