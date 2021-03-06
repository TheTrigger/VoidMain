﻿namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public interface ICollectionAdapter
    {
        object Collection { get; }
        int Count { get; }
        object GetValue(int index);
        void SetValue(int index, object value);
    }
}
