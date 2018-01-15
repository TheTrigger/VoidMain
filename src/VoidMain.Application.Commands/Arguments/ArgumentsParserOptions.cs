using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParserOptions
    {
        public ICollectionConstructor ArrayConstructor { get; set; }
        public Dictionary<Type, ICollectionConstructor> CollectionConstructors { get; set; }
        public IValueParser DefaultParser { get; set; }
        public Dictionary<Type, IValueParser> ValueParsers { get; set; }

        public ArgumentsParserOptions()
        {
            CollectionConstructors = new Dictionary<Type, ICollectionConstructor>();
            ValueParsers = new Dictionary<Type, IValueParser>();
        }
    }
}
