using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Paradigms.Application.Commands
{
    public class CdCommand : ICommand
    {
        public string Name { get; set; }

        public async Task<CommandResult> Execute(string command, string currentDir)
        {
            var commandResult = new CommandResult();

            var regex = new Regex(@"([A-Z]:\\w*)|(^\s{0,}\.{1,1}/)|(^\s{0,}\.{2,2}/$)");
            if (!regex.IsMatch(command)) 
            {
                commandResult.Errors.Add("Некорректные параметры команды");
                return commandResult;
            }

            var c = command.Trim();

            if (c =="../") 
            {
                commandResult.CurrentDir = Directory.GetParent(currentDir).FullName;
                return commandResult;
            }
            if (c.StartsWith("./")) 
            {
                var childDir = currentDir + $"\\{c[2..]}";
                var isExists = Directory.Exists(childDir);
                if (!isExists) 
                {
                    commandResult.Errors.Add("Дирректории не существует");
                    return commandResult;
                }

                commandResult.CurrentDir = childDir;
                return commandResult;
            }

            if (!Directory.Exists(c))
            {
                commandResult.Errors.Add("Дирректории не существует");
                return commandResult;
            }
            else 
            {
                commandResult.CurrentDir = c;
                return commandResult;
            }
        }
    }
}
