
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Repositories;

namespace PhoneBook.Infrastructure.Persistence;

public class InMemoryContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts = new();
    private readonly object _lock = new();

    public Task AddAsync(Contact contact)
    {
        lock (_lock)
        {
            _contacts.Add(contact);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Contact contact)
    {
        lock (_lock)
        {
            var existing = _contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (existing != null)
            {
                _contacts.Remove(existing);
                _contacts.Add(contact);
            }
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        lock (_lock)
        {
            _contacts.RemoveAll(c => c.Id == id);
        }
        return Task.CompletedTask;
    }

    public Task<Contact?> GetByIdAsync(Guid id)
    {
        lock (_lock)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(contact);
        }
    }

    public Task<IEnumerable<Contact>> GetByTagAsync(string tag)
    {
        lock (_lock)
        {
            var results = _contacts
                .Where(c => c.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult<IEnumerable<Contact>>(results);
        }
    }

    public Task<IEnumerable<Contact>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IEnumerable<Contact>>(_contacts.ToList());
        }
    }
}