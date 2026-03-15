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

            var parts = input.Trim().Split(' ');
            var cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "help":
                    Console.WriteLine("help, clear, min <text>, make user <name>");
                    break;
                case "clear":
                    Console.Clear();
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
                        catch
                        {
                            Console.WriteLine("Error: could not save.");
                        }
                    }
                    break;
            }
        }
    }
}
