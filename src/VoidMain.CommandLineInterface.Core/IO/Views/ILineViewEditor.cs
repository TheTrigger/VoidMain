using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineInterface.IO.Views
{
    public interface ILineViewEditor
    {
        Task Edit(ILineView lineView, CancellationToken token = default(CancellationToken));
    }
}
