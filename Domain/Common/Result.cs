namespace PhoneBook.Domain.Common;
using PhoneBook.Domain.Errors;

public class Result
{
    public bool IsSuccess { get; }
    public DomainErrorCode ErrorCode { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool success, DomainErrorCode errorCode)
    {
        IsSuccess = success;
        ErrorCode = errorCode;
    }

    public static Result Success() => new(true, DomainErrorCode.None);
    public static Result Failure(DomainErrorCode errorCode) => new(false, errorCode);
}

public class Result<T> : Result
{
    private readonly T? _value;
    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("مقدار خروجی برای نتیجه ناموفق در دسترس نیست.");

    protected Result(T? value, bool success, DomainErrorCode errorCode) : base(success, errorCode)
    {
        _value = value;
    }

    public static Result<T> Success(T value) => new(value, true, DomainErrorCode.None);
    public new static Result<T> Failure(DomainErrorCode errorCode) => new(default, false, errorCode);
}