namespace VoidMain.CommandLineInterface.History
{
    public interface ICommandsHistoryManager
    {
        int Count { get; }
        int MaxCount { get; }
        bool TryGetPrevCommand(out string command);
        bool TryGetNextCommand(out string command);
        void AddCommand(string command);
        void Clear();
    }
}
