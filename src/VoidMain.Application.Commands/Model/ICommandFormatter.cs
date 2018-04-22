namespace VoidMain.Application.Commands.Model
{
    public interface ICommandFormatter
    {
        string FormatCommand(CommandModel command);
        string FormatCommandName(CommandName commandName);
        string FormatArgument(ArgumentModel argument);
    }
}