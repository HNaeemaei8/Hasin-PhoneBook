using PhoneBook.Domain.Entities;
namespace PhoneBook.Domain.Repositories;


public interface IContactRepository
{
    Task AddAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(Guid id);
    Task<Contact?> GetByIdAsync(Guid id);
    Task<IEnumerable<Contact>> GetByTagAsync(string tag);
    Task<IEnumerable<Contact>> GetAllAsync();
}
