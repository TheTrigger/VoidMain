using System;
using System.Collections.Generic;
using System.Text;
using VoidMain.CommandLineInterface.Internal;

namespace VoidMain.CommandLineInterface.IO.Templates
{
    public class MessageTemplateParser : IMessageTemplateParser
    {
        private const char EndOfInput = '\0';

        public MessageTemplate Parse(string messageTemplate)
        {
            if (messageTemplate == null)
            {
                throw new ArgumentNullException(nameof(messageTemplate));
            }
            if (messageTemplate.Length == 0)
            {
                return new MessageTemplate(Array.Empty<MessageTemplate.Token>());
            }

            var chars = new CharsReadOnlyList(messageTemplate);
            var cursor = new ElementsCursor<char>(chars, EndOfInput);
            var tokens = new List<MessageTemplate.Token>();

            while (!cursor.IsAtTheEnd())
            {
                switch (cursor.Peek())
                {
                    case '{':
                        if (cursor.Peek(1) == '{')
                        {
                            // Escaped opening bracket
                            tokens.Add(ScanTextToken(cursor));
                        }
                        else
                        {
                            // Argument
                            tokens.Add(ScanArgumentToken(messageTemplate, cursor));
                        }
                        break;
                    case '}':
                        if (cursor.Peek(1) == '}')
                        {
                            // Escaped closing bracket
                            tokens.Add(ScanTextToken(cursor));
                        }
                        else
                        {
                            throw new FormatException($"Unescaped closing bracket at symbol {cursor.Position}.");
                        }
                        break;
                    default:
                        tokens.Add(ScanTextToken(cursor));
                        break;
                }
            }

            return new MessageTemplate(tokens.ToArray());
        }

        private MessageTemplate.TextToken ScanTextToken(ElementsCursor<char> cursor)
        {
            var text = new StringBuilder(128);

            while (!cursor.IsAtTheEnd())
            {
                char c = cursor.Peek();
                if (c == '{')
                {
                    if (cursor.Peek(1) == '{')
                    {
                        // Escaped opening bracket
                        text.Append(c);
                        cursor.MoveNext(2);
                    }
                    else
                    {
                        // Beginning of an argument
                        break;
                    }
                }
                else if (c == '}')
                {
                    if (cursor.Peek(1) == '}')
                    {
                        // Escaped closing bracket
                        text.Append(c);
                        cursor.MoveNext(2);
                    }
                    else
                    {
                        // Unescaped closing bracket
                        // Error handled in the top method
                        break;
                    }
                }
                else
                {
                    text.Append(c);
                    cursor.MoveNext();
                }
            }

            return new MessageTemplate.TextToken(text.ToString());
        }

        private MessageTemplate.ArgumentToken ScanArgumentToken(string messageTemplate, ElementsCursor<char> cursor)
        {
            Skip(cursor, '{');

            var index = ScanArgumentIndex(messageTemplate, cursor);
            var alignment = ScanArgumentAlignment(messageTemplate, cursor);
            var format = ScanArgumentFormat(messageTemplate, cursor);

            if (!Skip(cursor, '}'))
            {
                throw new FormatException($"Missing closing bracket at symbol {cursor.Position}.");
            }

            return new MessageTemplate.ArgumentToken(index, alignment, format);
        }

        private int ScanArgumentIndex(string messageTemplate, ElementsCursor<char> cursor)
        {
            int start = cursor.Position;
            int length = 0;

            while (true)
            {
                ValidateArgumentEnd(cursor);

                char c = cursor.Peek();

                if (c == '}' || c == ',' || c == ':')
                {
                    if (length == 0)
                    {
                        throw new FormatException($"Missing argument name at symbol {cursor.Position}.");
                    }

                    string index = messageTemplate.Substring(start, length);

                    try
                    {
                        return Int32.Parse(index);
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException($"Invalid argument index at symbol {start}.", ex);
                    }
                }
                else
                {
                    length++;
                    cursor.MoveNext();
                }
            }
        }

        private int ScanArgumentAlignment(string messageTemplate, ElementsCursor<char> cursor)
        {
            if (!Skip(cursor, ','))
            {
                return 0;
            }

            int start = cursor.Position;
            int length = 0;

            while (true)
            {
                ValidateArgumentEnd(cursor);

                char c = cursor.Peek();

                if (c == '}' || c == ':')
                {
                    if (length == 0)
                    {
                        throw new FormatException($"Missing argument alignment at symbol {cursor.Position}.");
                    }

                    string alignment = messageTemplate.Substring(start, length);

                    try
                    {
                        return Int32.Parse(alignment);
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException($"Invalid argument alignment at symbol {start}.", ex);
                    }
                }
                else
                {
                    length++;
                    cursor.MoveNext();
                }
            }
        }

        private string ScanArgumentFormat(string messageTemplate, ElementsCursor<char> cursor)
        {
            if (!Skip(cursor, ':'))
            {
                return null;
            }

            int start = cursor.Position;
            int length = 0;

            while (true)
            {
                ValidateArgumentEnd(cursor);

                char c = cursor.Peek();

                if (c == '}')
                {
                    if (length == 0)
                    {
                        throw new FormatException($"Missing argument format at symbol {cursor.Position}.");
                    }

                    string format = messageTemplate.Substring(start, length);
                    return format;
                }
                else
                {
                    length++;
                    cursor.MoveNext();
                }
            }
        }

        private static bool Skip(ElementsCursor<char> cursor, char value)
        {
            if (cursor.Peek() == value)
            {
                cursor.MoveNext();
                return true;
            }
            return false;
        }

        private static void ValidateArgumentEnd(ElementsCursor<char> cursor)
        {
            if (cursor.IsAtTheEnd())
            {
                throw new FormatException($"Missing closing bracket at symbol {cursor.Position}.");
            }
        }
    }
}
