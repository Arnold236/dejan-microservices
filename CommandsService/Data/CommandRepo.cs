using CommandsService.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;
        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }
        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Platforms.Add(plat);

        }

        public bool ExternalPlatfomrExists(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.Id == externalPlatformId);
        }
        public IEnumerable<Platform> GetAllPlatform()
        {
            return _context.Platforms.ToList();
        }
        public Command GetCommands(int platformId, int commandId)
        {
            return _context.Commands
                    .Where(c => c.PlatformId == platformId && c.Id == commandId )
                    .FirstOrDefault();
        }
        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                    .Where(c => c.PlatformId == platformId)
                    .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}