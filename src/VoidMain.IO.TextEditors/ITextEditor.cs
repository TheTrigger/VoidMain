using System.Threading;
using System.Threading.Tasks;
using VoidMain.IO.Keyboard;

namespace VoidMain.IO.TextEditors
{
    public interface ITextEditor<TText>
         where TText : class
    {
        event EditingPhase<TText>? StartEditing;
        event EditingPhase<TText>? StopEditing;

        void BindToAnyKey(KeyHandler<TText> handler);
        void UnbindFromAnyKey(KeyHandler<TText> handler);
        void UnbindFromAnyKey();

        void BindToKey(KeyInfo key, KeyHandler<TText> handler);
        void UnbindFromKey(KeyInfo key, KeyHandler<TText> handler);
        void UnbindFromKey(KeyInfo key);

        void UnbindFromAllKeys();

        Task EditAsync(TText text, IEditingEventsListener? eventsListener = null, CancellationToken token = default);
    }
}
