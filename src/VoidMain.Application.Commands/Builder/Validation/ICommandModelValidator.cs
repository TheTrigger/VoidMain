using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder.Validation
{
    public interface ICommandModelValidator
    {
        ValidationResult Validate(CommandModel command);
    }
}
