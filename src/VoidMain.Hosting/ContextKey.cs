namespace VoidMain.Hosting
{
    public static class ContextKey
    {
        private const string Namespace = "VoidMain.";
        public static readonly string CommandLine = Namespace + "CommandLine";
        public static readonly string CommandName = Namespace + "CommandName";
        public static readonly string CommandOptions = Namespace + "CommandOptions";
        public static readonly string CommandOperands = Namespace + "CommandOperands";
        public static readonly string CommandCancelled = Namespace + "CommandCancelled";
        public static readonly string Output = Namespace + "Output";
    }
}
