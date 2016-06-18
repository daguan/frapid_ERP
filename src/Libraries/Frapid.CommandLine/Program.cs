using System;
using System.Diagnostics;
using System.Linq;
using frapid.Commands;

namespace frapid
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var loader = new AssemblyLoader();
            loader.PreLoad();

            bool exit = false;

            string command;

            if(args != null &&
               args.Any())
            {
                command = string.Join(" ", args);
                CommandProcessor.Process(command);
                exit = true;
            }

            while(!exit)
            {
                command = FrapidConsole.ReadLine();
                exit = GotQuitSignalFrom(command);
                command = CheckClearSignal(command);

                if(!exit)
                {
                    CommandProcessor.Process(command);
                }
            }
        }

        private static string CheckClearSignal(string command)
        {
            var candidates = new[]
                             {
                                 "cls",
                                 "clear"
                             };
            bool clear = candidates.Contains(command.ToLower());

            if(clear)
            {
                Console.Clear();
                return string.Empty;
            }

            //Pass
            return command;
        }

        private static bool GotQuitSignalFrom(string command)
        {
            return command.ToLower().Equals("exit");
        }
    }
}