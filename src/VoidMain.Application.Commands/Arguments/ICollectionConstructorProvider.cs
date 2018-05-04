﻿using System;

namespace VoidMain.Application.Commands.Arguments
{
    public interface ICollectionConstructorProvider
    {
        bool IsCollection(Type collectionType);
        bool TryGetConstructor(Type collectionType, IServiceProvider services, out ICollectionConstructor constructor);
    }
}
