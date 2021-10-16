using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Paradigms.Application
{
    public static class DirectoryWorker
    {
        public static string ExecuteCd(string command, string currentDir) 
        {
            var res = GetRegexValue(@"(cd [A-Z]:\\w*)|(cd ../$)|(cd ./)",command);
            if (res == null) 
            {
                throw new Exception("Некорректный формат команды");
            }
            foreach (Match m in res) 
            {
                switch (m.Value)
                {
                    case "cd ../":
                        {
                            return Directory.GetParent(currentDir).FullName;
                        }
                    case "cd ./":
                        {
                            var childDir = command[5..];
                            var resDir = currentDir + $"\\{childDir}";
                            return Directory.Exists(resDir) ? resDir : null;
                        }
                    default:
                        {
                            var resDir = command[3..];
                            return Directory.Exists(resDir) ? resDir : null;
                        }
                }
            }
            return null;
        }

        public static void ExecuteCf(string command, string currentDir) 
        {
            var res = GetRegexValue(@"([A-Z,a-z,А-Я,а-я,1-9,\.,\-,\*,\!,\?,_]+)|(\*)|(\w*)", command);
            if (res == null)
            {
                throw new Exception("Некорректный формат команды");
            }
            int i = 1;
            string format = "";
            foreach (Match m in res) 
            {
                if (string.IsNullOrEmpty(m.Value) || m.Value == "cf") continue;

                if (i == 1) 
                {
                    format = m.Value;
                    i++;
                    continue;
                }

                if (m.Value == "*") 
                {
                    ChangeFilesFormat(currentDir,format);
                    continue;
                }
                var dir = currentDir + @"\" + m.Value;
                if (File.Exists(dir)) 
                {
                    ChangeFileFormat(dir, format);
                }
            }
        }

        public static async Task<List<Info>> ExecuteLs(string command, string currentDir) 
        {
            var res = GetRegexValue(@"(ls$)|(ls -l$)", command);
            if (res == null)
            {
                throw new Exception("Некорректный формат команды");
            }
            foreach (Match m in res) 
            {
                if (m.Value== "ls")
                {
                    return await GetDirList(currentDir);
                }

                if (m.Value == "ls -l")
                {
                    return await GetDirList(currentDir, true);
                }
            }
            return null;
        }

        private static MatchCollection GetRegexValue(string regex, string input) 
        {
            Regex reg = new Regex(regex);
            MatchCollection matches = reg.Matches(input);
            if (matches.Count == 0)
            {
                return null;
            }
            return matches;
        }

        private async static Task<List<Info>> GetDirList(string currentDir, bool isKey = false) 
        {
            var list = new List<Info>();
            var dirInfo = new DirectoryInfo(currentDir);
            foreach (FileSystemInfo info in dirInfo.GetFileSystemInfos()) 
            {
                var typeName = info.GetType().Name;
                list.Add(new Info
                {
                    Name = info.Name,
                    Type = isKey ? typeName[0..^4] : "",
                    Size = isKey ? info is FileInfo ? (info as FileInfo).Length.ToString() : (await CalcDirSize(info.FullName)).ToString() : ""
                }) ;
            }
            return list;
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

        private static void ChangeFilesFormat(string currentDir, string format) 
        {
            var dir = new DirectoryInfo(currentDir);
            foreach (var f in dir.GetFiles()) 
            {
                File.Move(f.FullName, Path.ChangeExtension(f.FullName, format));
            }
        }

        private static void ChangeFileFormat(string fileName, string format) 
        {
            File.Move(fileName, Path.ChangeExtension(fileName, format));
        }
    }
}
