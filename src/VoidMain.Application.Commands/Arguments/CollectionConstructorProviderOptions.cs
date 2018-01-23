﻿using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments
{
    public class CollectionConstructorProviderOptions
    {
        public ICollectionConstructor ArrayConstructor { get; set; }
        public Dictionary<Type, ICollectionConstructor> CollectionConstructors { get; set; }

        public CollectionConstructorProviderOptions()
        {
            CollectionConstructors = new Dictionary<Type, ICollectionConstructor>();
        }
    }
}
