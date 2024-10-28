namespace _3_Shared.Middleware.Exceptions;

public class ImageListNotFoundException : Exception
{
    public ImageListNotFoundException() { }

    public ImageListNotFoundException(string message)
        : base(message) { }

    public ImageListNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}