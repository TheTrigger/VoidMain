using System.Threading;
using VoidMain.IO.Keyboard;

namespace VoidMain.IO.TextEditors
{
    public interface IEditingContext<TText>
    {
        TText Text { get; }
        CancellationToken Cancellation { get; }

        KeyInput Input { get; }
        uint InputId { get; }
        bool IsKeyBound { get; }

        bool IsModalKeyHandlingEnabled { get; }
        bool IsModalKeyHandlingInProgress { get; }
        void EnterModalKeyHandling(KeyHandler<TText> handler);
        void ExitModalKeyHandling(bool isKeyHandled);

        bool IsCloseRequested { get; }
        void CloseEditor();
    }
}
