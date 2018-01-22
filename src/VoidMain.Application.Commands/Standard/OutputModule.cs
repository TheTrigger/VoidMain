using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Standard
{
    [Module(Name = "")]
    public class OutputModule
    {
        public void Clear(ICommandLineOutput output)
        {
            output.Clear();
        }
    }
}
