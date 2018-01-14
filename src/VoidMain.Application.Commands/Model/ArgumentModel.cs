using System;
using System.Reflection;
using System.Text;

namespace VoidMain.Application.Commands.Model
{
    public class ArgumentModel
    {
        public ParameterInfo Parameter { get; set; }
        public ArgumentKind Kind { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool Optional { get; set; }
        public object DefaultValue { get; set; }
        public Type ValueParser { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(Type.Name)
                .Append(' ')
                .Append(Name);

            if (Optional)
            {
                sb.Append('?');
            }
            if (!ReferenceEquals(DefaultValue, null))
            {
                sb.Append(" = ").Append(DefaultValue);
            }

            return sb.ToString();
        }
    }
}
