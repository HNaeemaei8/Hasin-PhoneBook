using PhoneBook.Domain.Common;
using PhoneBook.Domain.Errors;

namespace PhoneBook.Domain.ValueObjects;

public class PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number) || number.Length < 10)
        {
            return Result<PhoneNumber>.Failure(DomainErrorCode.InvalidPhoneNumber);
        }

        return Result<PhoneNumber>.Success(new PhoneNumber(number));
    }

    public override bool Equals(object? obj) => obj is PhoneNumber other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}