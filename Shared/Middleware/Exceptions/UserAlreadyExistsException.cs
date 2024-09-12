namespace _3_Shared.Middleware.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() { }

    public UserAlreadyExistsException(string message)
        : base(message) { }

    public UserAlreadyExistsException(string message, Exception inner)
        : base(message, inner) { }
}