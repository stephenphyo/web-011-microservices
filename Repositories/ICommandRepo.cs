using CommandService.Models;

namespace CommandService.Repositories
{
    public interface ICommandRepo
    {
        IEnumerable<Command> GetAllCommandsForPlatform(int platformId);
        Command? GetById(int platformId, int commandId);
        void Create(int platformId, Command command);
        bool SaveChanges();
    }
}