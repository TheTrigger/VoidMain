using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineInterface.IO.Views;

namespace VoidMain.CommandLineInterface.IO
{
    public interface ILineViewEditor
    {
        Task Edit(ILineView lineView, CancellationToken token = default(CancellationToken));
    }
}
