namespace VoidMain.CommandLineIinterface
{
    public class CommandLineViewOptions
    {
        public CommandLineViewType ViewType { get; set; }
        public char MaskSymbol { get; set; }

        public static CommandLineViewOptions Normal =>
            new CommandLineViewOptions
            {
                ViewType = CommandLineViewType.Normal,
            };

        public static CommandLineViewOptions Masked(char maskSymbol)
        {
            return new CommandLineViewOptions
            {
                ViewType = CommandLineViewType.Masked,
                MaskSymbol = maskSymbol
            };
        }

        public static CommandLineViewOptions Hidden =>
            new CommandLineViewOptions
            {
                ViewType = CommandLineViewType.Hidden,
            };
    }
}
