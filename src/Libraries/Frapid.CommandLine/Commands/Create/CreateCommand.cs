using System;
using System.Linq;

namespace frapid.Commands.Create
{
    public abstract class CreateCommand : ICommand
    {
        public abstract string Syntax { get; }
        public string Line { get; set; }
        public string CommandName { get; } = "create";
        public abstract string Name { get; }
        public abstract bool IsValid { get; set; }

        public abstract void Initialize();
        public abstract void Validate();
        public abstract void ExecuteCommand();

        public void Execute()
        {
            string resourceType = this.Line.Split(' ')[1];

            var iType = typeof(CreateCommand);
            var members = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance);

            var member = members.Cast<CreateCommand>().FirstOrDefault(m => m.Name == resourceType);

            if (member != null)
            {
                member.Line = this.Line;
                member.Initialize();
                member.Validate();
                member.ExecuteCommand();
            }
        }
    }
}