namespace AspNetAssignments.Exceptions;

/// <summary>
/// Виняток, що викидається коли запитуваний ресурс або сутність не знайдено
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
