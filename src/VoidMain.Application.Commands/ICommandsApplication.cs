using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoidMain.Application.Commands
{
    public interface ICommandsApplication
    {
        Task ExecuteCommand(Dictionary<string, object> context);
    }
}
