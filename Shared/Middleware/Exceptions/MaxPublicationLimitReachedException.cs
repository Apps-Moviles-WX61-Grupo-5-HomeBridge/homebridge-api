namespace _3_Shared.Middleware.Exceptions;

public class MaxPublicationLimitReachedException : Exception
{
    public MaxPublicationLimitReachedException() { }

    public MaxPublicationLimitReachedException(string message)
        : base(message) { }

    public MaxPublicationLimitReachedException(string message, Exception inner)
        : base(message, inner) { }
}