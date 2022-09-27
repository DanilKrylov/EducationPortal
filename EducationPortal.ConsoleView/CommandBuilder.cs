using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.ConsoleView
{
    public class CommandBuilder
    {
        private bool _builded = false;
        private readonly Dictionary<string, Action<string>> _commands = new Dictionary<string, Action<string>>();

        public void AddCommand(string commandName, Action<string> function)
        {
            if (_builded)
            {
                throw new InvalidOperationException("CommandBuilder have allready builded");
            }

            if (_commands.Keys.Contains(commandName))
            {
                throw new ArgumentException("Command already added");
            }

            _commands[commandName] = function;
        }

        public void Invoke(string commandName, string functionParam)
        {
            if (!_builded)
            {
                throw new InvalidOperationException("CommandBuilder is not builded");
            }

            if (!_commands.Keys.Contains(commandName))
            {
                throw new ArgumentException("not found this command");
            }

            _commands[commandName].Invoke(functionParam);
        }

        public void Build()
        {
            if (_builded)
            {
                throw new InvalidOperationException("CommandBuilder have allready builded");
            }

            _builded = true;
        }
    }
}
