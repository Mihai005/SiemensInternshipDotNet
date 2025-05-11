public class LogicalException : Exception
{
    public LogicalException(string message) : base(message) { }

    public LogicalException(string message, Exception inner) : base(message, inner) { }
}

