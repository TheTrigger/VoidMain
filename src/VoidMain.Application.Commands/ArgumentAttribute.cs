using System;
using System.Text;

namespace VoidMain.Application.Commands
{
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
}
