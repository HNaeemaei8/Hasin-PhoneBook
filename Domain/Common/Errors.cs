namespace PhoneBook.Domain.Errors;

public enum DomainErrorCode
{
    None = 0,
    InvalidPhoneNumber,
    NameIsRequired,
    LastNameIsRequired,
    ContactNotFound,
    ServerError
}
