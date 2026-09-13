namespace SmartStock.Application.Common;

public abstract class AppException : Exception
{
    public string Code { get; }

    protected AppException(string code, string message)
        : base(message)
    {
        Code = code;
    }
}

public sealed class ValidationException : AppException
{
    public ValidationException(string message)
        : base("VALIDATION_ERROR", message)
    {
    }
}

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base("NOT_FOUND", message)
    {
    }
}

public sealed class ConflictException : AppException
{
    public ConflictException(string message)
        : base("CONFLICT", message)
    {
    }
}

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base("UNAUTHORIZED", message)
    {
    }
}

public static class ValidationRules
{
    public static string RequireText(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{fieldName} is required.");

        return value.Trim();
    }

    public static string RequireValue(string? value, string fieldName)
    {
        return RequireText(value, fieldName);
    }
}
