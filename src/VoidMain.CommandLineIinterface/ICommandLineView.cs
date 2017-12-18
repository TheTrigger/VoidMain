namespace VoidMain.CommandLineIinterface.Console
{
    public interface ICommandLineView : ICommandLineReadOnlyView
    {
        void Move(int offset);
        void MoveTo(int position);

        void Delete(int count);
        void ClearAll();

        void Type(char value);
        void TypeOver(char value);

        void Type(string value);
        void TypeOver(string value);
    }
}
