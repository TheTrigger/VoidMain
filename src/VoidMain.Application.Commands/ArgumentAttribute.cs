using System;
using System.Text;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public abstract class ArgumentAttribute : Attribute
    {
        public string Name { get; set; }
        public char? Alias { get; set; }
        public string Description { get; set; }
        public bool? Optional { get; set; }
        public object DefaultValue { get; set; }
        public Type ValueParser { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(Name);
            if (Optional.HasValue && Optional.Value)
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

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class OptionAttribute : ArgumentAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class OperandAttribute : ArgumentAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceAttribute : Attribute
    {
    }
}
