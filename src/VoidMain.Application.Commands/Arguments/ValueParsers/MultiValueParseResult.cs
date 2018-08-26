namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class MultiValueParseResult
    {
        public object Value { get; set; }
        public int ValuesUsed { get; set; }

        public MultiValueParseResult(object value, int valuesUsed)
        {
            Value = value;
            ValuesUsed = valuesUsed;
        }
    }
}
