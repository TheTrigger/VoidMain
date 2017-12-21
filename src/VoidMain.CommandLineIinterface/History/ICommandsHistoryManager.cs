namespace VoidMain.CommandLineIinterface.History
{
    public interface ICommandsHistoryManager
    {
        bool TryGetPrevCommand(out string command);
        bool TryGetNextCommand(out string command);
        void AddCommand(string command);
    }
}
