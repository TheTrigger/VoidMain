using System;
using System.Linq;

namespace VoidMain.CommandLineIinterface.Internal
{
    public static class ArgsUtil
    {
        public static string ToCommandLine(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return String.Empty;
            }

            return String.Join(" ", args.Select(EscapeArg));
        }

        public static string EscapeArg(string arg)
        {
            bool hasQuotes = false;
            bool hasWhitespace = false;

            foreach (var c in arg)
            {
                if (c == '"')
                {
                    hasQuotes = true;
                    if (hasWhitespace) break;
                }
                else if (Char.IsWhiteSpace(c))
                {
                    hasWhitespace = true;
                    if (hasQuotes) break;
                }
            }

            if (hasQuotes)
            {
                arg = arg.Replace("\"", "\\\"");
            }
            if (hasWhitespace)
            {
                arg = "\"" + arg + "\"";
            }

            return arg;
        }
    }
}
