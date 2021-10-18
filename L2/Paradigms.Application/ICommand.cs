using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Paradigms.Application
{
    public interface ICommand
    {
        public string Name { get; set; }
        public Task<CommandResult> Execute(string command, string currentDir);
    }
}
