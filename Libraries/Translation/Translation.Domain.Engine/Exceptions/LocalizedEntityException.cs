namespace Translation.Domain.Engine.Exceptions;
/// <summary>
/// Exception type for domain exceptions.
/// </summary>
internal class LocalizedEntityException : Exception
{
    internal const string ErrorCode = nameof(ErrorCode);
    internal LocalizedEntityException() { }

    internal LocalizedEntityException(string message) : base(message) { }

    internal LocalizedEntityException(string message, string? errorCode): base(message)
    {
        Data.Add(ErrorCode, errorCode);
    }

    internal LocalizedEntityException(string message, Exception innerException) : base(message, innerException) { }
}
