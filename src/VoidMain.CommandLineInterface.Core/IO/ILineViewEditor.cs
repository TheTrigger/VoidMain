using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO
{
    public interface ILineViewEditor
    {
        Task Edit(ILineView lineView, CancellationToken token = default(CancellationToken));
    }
}
