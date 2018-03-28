using System;

namespace VoidMain.Application.Commands.Builder.Validation
{
    public class IdentifierValidator : IIdentifierValidator
    {
        private readonly string AllowedSymbols = "_-.";

        public bool IsValid(string identifier)
        {
            if (String.IsNullOrEmpty(identifier))
            {
                return false;
            }

            char first = identifier[0];
            if (!Char.IsLetter(first))
            {
                return false;
            }

            for (int i = 1; i < identifier.Length; i++)
            {
                char symbol = identifier[i];
                bool isValid = Char.IsLetterOrDigit(symbol)
                    || AllowedSymbols.IndexOf(symbol) > -1;
                if (!isValid)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
