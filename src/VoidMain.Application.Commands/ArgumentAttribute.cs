using System;
using System.Text;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public abstract class ParameterAttribute : Attribute
    {
        protected bool? _optional;
        public bool Optional
        {
            get => _optional ?? false;
            set => _optional = value;
        }
        public bool IsOptionalSet { get => _optional.HasValue; }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceAttribute : ParameterAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public abstract class ArgumentAttribute : ParameterAttribute
    {
        public string Name { get; set; }
        protected char? _alias;
        public char Alias
        {
            get => _alias ?? '\0';
            set => _alias = value;
        }
        public bool IsAliasSet { get => _alias.HasValue; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }
        public Type ValueParser { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(Name);
            if (_optional.HasValue && _optional.Value)
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
        public OptionAttribute() { }
        public OptionAttribute(string name) => Name = name;
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class OperandAttribute : ArgumentAttribute
    {
        public OperandAttribute() { }
        public OperandAttribute(string name) => Name = name;
    }
}
