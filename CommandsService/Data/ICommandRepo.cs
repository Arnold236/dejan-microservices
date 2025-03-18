using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        // Platforms
        IEnumerable<Platform> GetAllPlatform();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);
        bool ExternalPlatfomrExists(int ExternalPlatformId);
        
        // Comands

        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommands(int platformId, int commandId);
        void CreateCommand(int platformId, Command command );
     
    }
}