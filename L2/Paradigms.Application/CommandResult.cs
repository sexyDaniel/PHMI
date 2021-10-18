using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paradigms.Application
{
    public class CommandResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public List<Info> Result { get; set; }
        public string CurrentDir { get; set; }
    }
}
