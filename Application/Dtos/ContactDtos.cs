namespace PhoneBook.Application.Dtos;

public record CreateContactDto(string FirstName, string LastName, string PhoneNumber, string Tag);

public record UpdateContactDto(string FirstName, string LastName, string PhoneNumber, string Tag);

public record ContactDto(Guid Id, string FirstName, string LastName, string PhoneNumber, string Tag);
