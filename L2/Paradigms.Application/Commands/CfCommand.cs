using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Paradigms.Application.Commands
{
    public class CfCommand : ICommand
    {
        public string Name { get ; set ; }

        public async Task<CommandResult> Execute(string command, string currentDir)
        {
            var commandResult = new CommandResult();
            var args = command.Split().Where(a => a != "").ToList();
            if (args.Count < 2) 
            {
                commandResult.Errors.Add("Не все аргументы указаны");
                return commandResult;
            }
           
            if (args[1] == "*" && (args.Count > 2))
            {
                commandResult.Errors.Add("Указано больше аргументов. Или указан не тот аргумент");
                return commandResult;
            }

            ChangeFiles(currentDir,args[0],args.Skip(1).ToList(),commandResult);

            return commandResult;
        }

        private void ChangeFiles(string currentDir, string format,List<string> args, CommandResult result)
        {
            var files = args[0] == "*" ?
                new DirectoryInfo(currentDir).GetFiles().Select(f => f.FullName):
                args.Select(a=>currentDir+@"\"+a);

            foreach (var f in files)
            {
                try
                {
                    if (!File.Exists(f)) 
                    {
                        result.Errors.Add($"Файла {f} не существует");
                        continue;
                    }
                    File.Move(f, Path.ChangeExtension(f, format));
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Файл {f} уже существует");
                }

            }
            
        }
    }
}
