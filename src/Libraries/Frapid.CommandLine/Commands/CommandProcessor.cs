using System;
using System.Linq;

namespace frapid.Commands
{
    public static class CommandProcessor
    {
        public static void Process(string line)
        {
            string commandName = line.Split(' ')[0];
            var command = Get(commandName, line);

            command?.Execute();
        }

        private static ICommand Get(string commandName, string line)
        {
            var iType = typeof(ICommand);
            var members = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance);

            var command = members.Cast<ICommand>().FirstOrDefault(member => member.CommandName == commandName);

            if (command != null)
            {
                command.Line = line;
                return command;
            }

            return null;
        }

        public static void DisplayError(string syntax, string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error : " + format, args);

            Console.ForegroundColor = ConsoleColor.White;

            if (!string.IsNullOrWhiteSpace(syntax))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\r\nSyntax(es) : \r\n" + syntax);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}