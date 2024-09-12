namespace _3_Shared.Middleware.Exceptions;

public class PublicationNotFoundException : Exception
{
    public PublicationNotFoundException() { }

    public PublicationNotFoundException(string message)
        : base(message) { }

    public PublicationNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}