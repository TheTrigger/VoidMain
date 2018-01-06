namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineView : ICommandLineReadOnlyView
    {
        CommandLineViewType ViewType { get; }
        char MaskSymbol { get; }

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
