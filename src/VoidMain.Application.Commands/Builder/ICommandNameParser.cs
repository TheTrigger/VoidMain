using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public interface ICommandNameParser
    {
        CommandName Parse(string commandName);
    }
}
