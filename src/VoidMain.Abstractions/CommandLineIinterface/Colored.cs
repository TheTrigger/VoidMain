namespace VoidMain.CommandLineInterface
{
    public class Colored<T>
    {
        public T Value { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }

        public Colored(T value, Color foreground = null, Color background = null)
        {
            Value = value;
            Foreground = foreground;
            Background = background;
        }

        public override string ToString()
        {
            return Value?.ToString();
        }
    }
}
