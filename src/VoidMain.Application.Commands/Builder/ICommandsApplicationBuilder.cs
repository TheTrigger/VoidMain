namespace VoidMain.Application.Commands.Builder
{
    public interface ICommandsApplicationBuilder
    {
        void AddModule<TModule>();
        ICommandsApplication Build();
    }
}
