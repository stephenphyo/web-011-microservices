using CommandService.Data;
using CommandService.Models;

namespace CommandService.Repositories
{
    public class CommandRepo : ICommandRepo
    {
        /* Properties */
        private readonly AppDbContext _ctx;

        /* Constructor */
        public CommandRepo(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        /* Methods */
        public IEnumerable<Command> GetAllCommandsForPlatform(int platformId)
        {
            return _ctx.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.CommandLine)
                .ToList();
        }

        public Command? GetById(int platformId, int commandId)
        {
            return _ctx.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId)
                .FirstOrDefault();
        }

        public void Create(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.PlatformId = platformId;
            _ctx.Commands.Add(command);
        }

        public bool SaveChanges()
        {
            return _ctx.SaveChanges() >= 0;
        }
    }
}