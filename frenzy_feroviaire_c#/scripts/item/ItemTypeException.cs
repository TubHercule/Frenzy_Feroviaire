using System;
public class ItemTypeException : Exception
{
    public ItemTypeException()
    {
    }

    public ItemTypeException(string message)
        : base(message)
    {
    }

    public ItemTypeException(string message, Exception inner)
        : base(message, inner)
    {
    }
}