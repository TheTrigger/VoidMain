using System;
using System.Collections.Generic;
using System.Reflection;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class ModuleModelConstructor : IModuleModelConstructor
    {
        private const string ModuleSuffix = "Module";

        public bool IsModule(TypeInfo type)
        {
            return GetError(type) == null;
        }

        public ModuleModel Create(TypeInfo type)
        {
            Validate(type);

            var module = new ModuleModel();
            module.Type = type.AsType();
            module.Commands = new List<CommandModel>();

            var attr = type.GetCustomAttribute<ModuleAttribute>();
            if (attr == null)
            {
                module.Name = RemoveSuffix(type.Name, ModuleSuffix);
            }
            else
            {
                module.Name = attr.Name ?? RemoveSuffix(type.Name, ModuleSuffix);
                module.Description = attr.Description;
            }

            return module;
        }

        private string RemoveSuffix(string value, string suffix)
        {
            if (!value.EndsWith(suffix))
            {
                return value;
            }

            return value.Substring(0, value.Length - suffix.Length);
        }

        private void Validate(TypeInfo type)
        {
            string errorMessage = GetError(type);
            if (errorMessage != null)
            {
                throw new ArgumentException(
                    $"Invalid type {type}: {errorMessage}",
                    nameof(type));
            }
        }

        private string GetError(TypeInfo type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsClass)
            {
                return "The module must be a class.";
            }

            if (!type.IsPublic && !type.IsNestedPublic)
            {
                return "The module must be a public class.";
            }

            if (type.IsAbstract)
            {
                return "The module must be a non-abstract class.";
            }

            if (type.ContainsGenericParameters)
            {
                return "All generic parameters for the module must be specified.";
            }

            if (type.IsDefined(typeof(NonModuleAttribute)))
            {
                return "The module must not have a NonModule attribute.";
            }

            return null;
        }
    }
}
