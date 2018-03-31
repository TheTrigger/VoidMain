namespace VoidMain.CommandLineIinterface.History
{
    public interface ICommandsHistoryStorage
    {
        string[] Load();
        void Save(string[] commands);
    }
}
