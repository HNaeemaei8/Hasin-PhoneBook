using PhoneBook.Domain.Common;
using PhoneBook.Domain.Errors;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Entities;

public class Contact
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public string Tag { get; private set; } = null!;

    private Contact() { } 
    public static Result<Contact> Create(string firstName, string lastName, PhoneNumber phoneNumber, string tag)
    {
        var contact = new Contact
        {
            Id = Guid.NewGuid()
        };

        var setResult = contact.ValidateAndSet(firstName, lastName, phoneNumber, tag);
        if (setResult.IsFailure)
            return Result<Contact>.Failure(setResult.ErrorCode);

        return Result<Contact>.Success(contact);
    }

    public Result Update(string firstName, string lastName, PhoneNumber phoneNumber, string tag)
        => ValidateAndSet(firstName, lastName, phoneNumber, tag);

    private Result ValidateAndSet(string firstName, string lastName, PhoneNumber phoneNumber, string tag)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure(DomainErrorCode.NameIsRequired);

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure(DomainErrorCode.LastNameIsRequired);

        if (phoneNumber is null)
            return Result.Failure(DomainErrorCode.InvalidPhoneNumber);

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Tag = string.IsNullOrWhiteSpace(tag) ? "General" : tag;

        return Result.Success();
    }
}