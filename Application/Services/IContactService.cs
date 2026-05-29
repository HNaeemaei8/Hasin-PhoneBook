using PhoneBook.Application.Dtos;
using PhoneBook.Domain.Common;

namespace PhoneBook.Application.Services;

public interface IContactService
{
    Result<ContactDto> CreateContact(CreateContactDto dto);
    Result UpdateContact(Guid id, UpdateContactDto dto);
    Result DeleteContact(Guid id);
    Result<IEnumerable<ContactDto>> GetContactsByTag(string tag);
    Result<IEnumerable<ContactDto>> GetAllContacts();
}