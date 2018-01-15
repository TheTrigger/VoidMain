using System;

namespace VoidMain.Application.Commands.Arguments
{
    public interface IValueParser
    {
        object Parse(string text, Type valueType);
    }
}
