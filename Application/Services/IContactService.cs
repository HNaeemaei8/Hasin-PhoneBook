using PhoneBook.Application.Dtos;
using PhoneBook.Domain.Common;

namespace PhoneBook.Application.Services;

public interface IContactService
{
    Task<Result<ContactDto>> CreateContactAsync(CreateContactDto dto);
    Task<Result> UpdateContactAsync(Guid id, UpdateContactDto dto);
    Task<Result> DeleteContactAsync(Guid id);
    Task<Result<IEnumerable<ContactDto>>> GetContactsByTagAsync(string tag);
    Task<Result<IEnumerable<ContactDto>>> GetAllContactsAsync();
}