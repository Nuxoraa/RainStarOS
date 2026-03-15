using System;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;

namespace RainStar
{
    public class Kernel : Sys.Kernel
    {
        private string _username = "user";
        private string _currentDir = @"0:\";
        private static readonly string _savePath = @"0:\username.txt";
        private CosmosVFS _vfs;

        protected override void BeforeRun()
        {
            _vfs = new CosmosVFS();
            VFSManager.RegisterVFS(_vfs);
            _vfs.Initialize(false);

            try
            {
                if (File.Exists(_savePath))
                    _username = File.ReadAllText(_savePath).Trim();
            }
            catch { }

            Console.Clear();
            Console.WriteLine("Welcome To RainStar OS");
        }

        protected override void Run()
        {
            Console.Write(_username + "$ ");
            var input = Console.ReadLine();
            if (input == null) return;

            input = input.Trim();
            if (input == "") return;

            var parts = input.Split(' ');
            var cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "help":
                    Console.WriteLine("help, clear, reboot, min <text>");
                    Console.WriteLine("make user <name>, ls, mkdir <name>, mit <file>");
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "reboot":
                    Sys.Power.Reboot();
                    break;

                case "min":
                    if (parts.Length > 1)
                        Console.WriteLine(input.Substring(input.IndexOf(' ') + 1));
                    break;

                case "make":
                    if (parts.Length >= 3 && parts[1].ToLower() == "user")
                    {
                        _username = parts[2];
                        try
                        {
                            File.WriteAllText(_savePath, _username);
                            Console.WriteLine("Saved.");
                        }
                        catch { Console.WriteLine("Error: could not save."); }
                    }
                    break;

                case "ls":
                    try
                    {
                        var dirs = Directory.GetDirectories(_currentDir);
                        var files = Directory.GetFiles(_currentDir);
                        foreach (var d in dirs)
                            Console.WriteLine("[dir]  " + Path.GetFileName(d));
                        foreach (var f in files)
                            Console.WriteLine("[file] " + Path.GetFileName(f));
                    }
                    catch { Console.WriteLine("Error reading directory."); }
                    break;

                case "mkdir":
                    if (parts.Length >= 2)
                    {
                        try
                        {
                            Directory.CreateDirectory(_currentDir + parts[1]);
                            Console.WriteLine("Created.");
                        }
                        catch { Console.WriteLine("Error creating directory."); }
                    }
                    break;

                case "mit":
                    if (parts.Length >= 2)
                        RunEditor(_currentDir + parts[1]);
                    break;

                default:
                    Console.WriteLine("unknown command: " + cmd);
                    break;
            }
        }

        private void RunEditor(string path)
        {
            Console.Clear();
            Console.WriteLine("mit editor — " + path);
            Console.WriteLine("Type lines. Empty line = new line. Type ':w' to save, ':q' to quit.");
            Console.WriteLine("---");

            string content = "";
            try
            {
                if (File.Exists(path))
                {
                    content = File.ReadAllText(path);
                    Console.WriteLine(content);
                    Console.WriteLine("---");
                }
            }
            catch { }

            var buffer = content;

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null) continue;

                if (line == ":q")
                {
                    Console.Clear();
                    Console.WriteLine("Welcome To RainStar OS");
                    break;
                }
                else if (line == ":w")
                {
                    try
                    {
                        File.WriteAllText(path, buffer);
                        Console.WriteLine("Saved.");
                    }
                    catch { Console.WriteLine("Error saving."); }
                }
                else
                {
                    buffer += line + "\n";
                }
            }
        }
    }
}
