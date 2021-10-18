using System;
using System.IO;
using Paradigms.Application.Commands;
using Paradigms.Application;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = new List<ICommand>() 
            {
                new LsCommand(){Name = "ls" },
                new CdCommand(){Name = "cd" },
                new HelpCommand(){Name= "help" },
                new CfCommand(){Name="cf" }
            };
            var currentDir = Directory.GetCurrentDirectory();
            var input = "";
            while (input != "exit") 
            {
                Console.Write($"{currentDir}>");
                input = Console.ReadLine();
                var commandName = input.Split()[0];
                var command = commands.FirstOrDefault(c=>c.Name == commandName);
                if (command == null) 
                {
                    Console.WriteLine("Неверная команда");
                    continue;
                }

                var result =  command.Execute(input[commandName.Length..],currentDir).Result;
                if (result.Errors.Count != 0)
                {
                    Console.WriteLine(string.Join('\n', result.Errors));
                    continue;
                }

                if (!string.IsNullOrEmpty(result.CurrentDir)) 
                {
                    currentDir = result.CurrentDir;
                    continue;
                }

                if (result.Result != null)
                {
                    foreach (var i in result.Result)
                    {
                        Console.WriteLine("{0, -50}  {1, -20}  {2}", i.Name, i.Type, i.Size);
                    }
                }
            }
        }
    }
}
