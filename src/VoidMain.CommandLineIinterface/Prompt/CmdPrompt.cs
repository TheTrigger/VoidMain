namespace VoidMain.CommandLineIinterface.IO
{
    public class CmdPrompt : ICommandLinePrompt
    {
        public string GetMessage() => "CMD> ";
    }
}
