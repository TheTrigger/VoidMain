using System;
using System.Collections.Generic;
using System.Linq;

namespace VoidMain.Application.Commands.Model
{
    public class CommandName
    {
        public IReadOnlyList<string> Parts { get; }

        public CommandName(IEnumerable<string> parts)
        {
            if (parts == null)
            {
                throw new ArgumentNullException(nameof(parts));
            }
            Parts = parts.ToArray();
        }

        private CommandName(string[] parts)
        {
            Parts = parts;
        }

        public override string ToString()
        {
            return String.Join(" ", Parts);
        }

        public static CommandName Parse(string routeString)
        {
            if (routeString == null)
            {
                throw new ArgumentNullException(nameof(routeString));
            }
            var parts = routeString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new CommandName(parts);
        }

        public static CommandName Combine(CommandName first, CommandName second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }
            int firstCount = first.Parts.Count;
            int secondCount = second.Parts.Count;
            var parts = new string[firstCount + secondCount];
            for (int i = 0; i < firstCount; i++)
            {
                parts[i] = first.Parts[i];
            }
            for (int i = firstCount; i < parts.Length; i++)
            {
                parts[i] = second.Parts[i];
            }
            return new CommandName(parts);
        }
    }
}
