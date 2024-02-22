using CommandService.Data;
using CommandService.Models;

namespace CommandService.Repositories
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
        public IEnumerable<Platform> GetAll()
        {
            return _ctx.Platforms.ToList();
        }

        public bool CheckPlatformExists(int platformId)
        {
            return _ctx.Platforms.Any(p => p.Id == platformId);
        }

        public bool CheckExternalPlatformExists(int extPlatformId)
        {
            return _ctx.Platforms.Any(p => p.ExternalId == extPlatformId);
        }

        public void Create(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _ctx.Platforms.Add(platform);
        }

        public bool SaveChanges()
        {
            return _ctx.SaveChanges() >= 0;
        }
    }
}