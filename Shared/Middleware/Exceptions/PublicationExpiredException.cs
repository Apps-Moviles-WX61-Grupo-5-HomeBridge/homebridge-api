namespace _3_Shared.Middleware.Exceptions;

public class PublicationExpiredException  : Exception
{
    public PublicationExpiredException() { }

    public PublicationExpiredException(string message)
        : base(message) { }

    public PublicationExpiredException(string message, Exception inner)
        : base(message, inner) { }
}