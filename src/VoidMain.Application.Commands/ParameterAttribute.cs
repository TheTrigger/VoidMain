using System;

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
        public bool IsSetOptional => _optional.HasValue;
    }
}
