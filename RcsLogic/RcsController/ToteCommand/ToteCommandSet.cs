using System.Collections.Generic;
using System.Linq;

namespace RcsLogic.RcsController.ToteCommand
{
    public class ToteCommandSet : IToteCommand
    {
        private readonly List<IToteCommand> _commands = new List<IToteCommand>();

        public void Add(IToteCommand command) => _commands.Add(command);
        public void Execute() => _commands.ForEach(command => command.Execute());
        public bool HasAnyCommands() => _commands.Any();
    }
}