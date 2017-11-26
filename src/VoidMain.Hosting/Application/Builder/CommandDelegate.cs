using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoidMain.Application.Builder
{
    public delegate Task CommandDelegate(Dictionary<string, object> context);
}
