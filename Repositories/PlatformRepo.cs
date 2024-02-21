using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Repositories
{
    public class PlatformRepo : IPlatformRepo
    {
        /* Properties */
        private readonly AppDbContext _ctx;

        /* Constructor */
        public PlatformRepo(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        /* Methods */
        public void Create(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _ctx.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAll()
        {
            return _ctx.Platforms.ToList();
        }

        public Platform? GetById(int id)
        {
            return _ctx.Platforms.FirstOrDefault(platform => platform.Id == id);
        }

        public bool SaveChanges()
        {
            return (_ctx.SaveChanges() >= 0);
        }
    }
}