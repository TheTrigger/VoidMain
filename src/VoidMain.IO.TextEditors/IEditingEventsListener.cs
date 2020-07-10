namespace VoidMain.IO.TextEditors
{
    public interface IEditingEventsListener
    {
        void OnModifying(bool isNextChangeAvailable);
        void OnModified(bool isNextChangeAvailable);
    }
}
