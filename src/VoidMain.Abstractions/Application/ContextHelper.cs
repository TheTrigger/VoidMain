using System;
using System.Collections.Generic;
using System.Threading;

namespace VoidMain.Application
{
    public static class ContextHelper
    {
        public static readonly string[] EmptyCommandName = Array.Empty<string>();
        public static readonly KeyValuePair<string, string>[] EmptyOptions = Array.Empty<KeyValuePair<string, string>>();
        public static readonly string[] EmptyOperands = Array.Empty<string>();

        public static void SetCommandLine(Dictionary<string, object> context, string commandLine)
        {
            context[ContextKey.CommandLine] = commandLine;
        }

        public static void SetCommandName(Dictionary<string, object> context, string[] commandName)
        {
            context[ContextKey.CommandName] = commandName;
        }

        public static void SetOptions(Dictionary<string, object> context, KeyValuePair<string, string>[] options)
        {
            context[ContextKey.CommandOptions] = options;
        }

        public static void SetOperands(Dictionary<string, object> context, string[] operands)
        {
            context[ContextKey.CommandOperands] = operands;
        }

        public static void SetCancelToken(Dictionary<string, object> context, CancellationToken token)
        {
            context[ContextKey.CommandCancelToken] = token;
        }

        public static bool TryGetCommandLine(Dictionary<string, object> context, out string commandLine)
        {
            return TryGetValue(context, ContextKey.CommandLine, out commandLine);
        }

        public static bool TryGetCommandName(Dictionary<string, object> context, out string[] commandName)
        {
            return TryGetValue(context, ContextKey.CommandName, out commandName);
        }

        public static bool TryGetOptions(Dictionary<string, object> context, out KeyValuePair<string, string>[] options)
        {
            return TryGetValue(context, ContextKey.CommandOptions, out options);
        }

        public static bool TryGetOperands(Dictionary<string, object> context, out string[] operands)
        {
            return TryGetValue(context, ContextKey.CommandOperands, out operands);
        }

        public static bool TryGetCancelToken(Dictionary<string, object> context, out CancellationToken token)
        {
            return TryGetValue(context, ContextKey.CommandCancelToken, out token);
        }

        private static bool TryGetValue<TValue>(Dictionary<string, object> context, string key, out TValue value)
        {
            if (context.TryGetValue(key, out var objValue))
            {
                value = (TValue)objValue;
                return true;
            }
            value = default(TValue);
            return false;
        }
    }
}
