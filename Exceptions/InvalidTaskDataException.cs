namespace TaskManagerApi.Exceptions;

/// <summary>
/// Thrown for validation rules that can't be expressed with data annotations
/// alone (e.g. "title must not be blank when supplied" on a partial PATCH).
/// Mapped to 400 by ExceptionHandlingMiddleware.
/// </summary>
public class InvalidTaskDataException : Exception
{
    public InvalidTaskDataException(string message) : base(message)
    {
    }
}
