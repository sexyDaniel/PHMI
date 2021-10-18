using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Paradigms.Application.Commands
{
    public class LsCommand : ICommand
    {
        public string Name { get; set; }

        public async Task<CommandResult> Execute(string command, string currentDir)
        {
            var commandResult = new CommandResult();
            var regex = new Regex(@"(^\s{0,}$)|(^\s{0,}-l$)");
            if (!regex.IsMatch(command))
            {
                commandResult.Errors.Add("Некорректные параметры команды");
                return await Task.Run(()=>commandResult); ;
            }

            if (command.Contains("-l"))
            {
                return await GetDirList(currentDir, commandResult, true);
            }
            else 
            {
                return await GetDirList(currentDir, commandResult);
            }
        }

        private async Task<CommandResult> GetDirList(string currentDir, CommandResult res, bool isKey = false)
        {
            res.Result = new List<Info>();
            var dirInfo = new DirectoryInfo(currentDir);
            foreach (FileSystemInfo info in dirInfo.GetFileSystemInfos())
            {
                var typeName = info.GetType().Name;
                res.Result.Add(new Info
                {
                    Name = info.Name,
                    Type = isKey ? typeName[0..^4] : "",
                    Size = isKey ? info is FileInfo ? (info as FileInfo).Length.ToString() : (await CalcDirSize(info.FullName)).ToString() : ""
                });
            }
            return res;
        }

        private static async Task<long> CalcDirSize(string catalog)
        {
            return await Task.Run(() =>
            {
                string[] fullfilesPath = Directory.GetFiles(catalog, "*.*", SearchOption.AllDirectories);
                long Size = 0;
                foreach (var item in fullfilesPath)
                {
                    FileInfo fi = new FileInfo(item);
                    Size += fi.Length;
                }
                return Size;
            });
        }
    }
}
