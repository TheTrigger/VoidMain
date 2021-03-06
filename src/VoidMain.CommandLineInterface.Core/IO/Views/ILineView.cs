﻿namespace VoidMain.CommandLineInterface.IO.Views
{
    public interface ILineView : IReadOnlyLineView
    {
        LineViewType ViewType { get; }

        void Move(int offset);
        void MoveTo(int position);

        void Delete(int count);
        void Clear();

        void Type(char value);
        void TypeOver(char value);

        void Type(string value);
        void TypeOver(string value);
    }
}
