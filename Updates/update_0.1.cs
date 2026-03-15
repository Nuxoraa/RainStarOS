using System;
using Sys = Cosmos.System;

namespace RainStar
{
    public class Kernel : Sys.Kernel
    {
        private string _username = "user";

        protected override void BeforeRun()
        {
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
                        _username = parts[2];
                    break;
            }
        }
    }
}
