using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigms.Application.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name { get ; set ; }

        public async Task<CommandResult> Execute(string command, string currentDir)
        {
            Console.WriteLine("cd <каталог> - переход в заданный каталог\ncd ../ - переход в родительский каталог\ncd ./<дирректория> - переход в следующую дирректорию\nls [-l] - вывод файлов и папок/[-l] с доп информацией\ncf <тип> <файл1> [<файл2>...] или [*] - смена форматов файлов, если * - всех файлов");
            return new CommandResult();
        }
    }
}
