namespace VoidMain.IO.TextEditors.TextLine
{
    public interface ITextEditorComponent<TText>
        where TText : class
    {
        void RegisterHandlers(ITextEditor<TText> editor);
        void UnregisterHandlers(ITextEditor<TText> editor);
    }
}
