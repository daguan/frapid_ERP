using System;
using System.Linq;
using frapid.Commands;

namespace frapid
{
    internal class Program
    {
        private static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                string command = FrapidConsole.ReadLine();
                exit = GotQuitSignalFrom(command);
                CheckClearSignal(command);

                if (!exit)
                {
                    CommandProcessor.Process(command);
                }
            }
        }

        private static void CheckClearSignal(string command)
        {
            var candidates = new[] {"cls", "clear"};
            bool clear = candidates.Contains(command.ToLower());

            if (clear)
            {
                Console.Clear();
            }
        }

        private static bool GotQuitSignalFrom(string command)
        {
            return command.ToLower().Equals("exit");
        }
    }
}