
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Repositories;

namespace PhoneBook.Infrastructure.Persistence;

public class InMemoryContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts = new();
    private readonly object _lock = new();

        public void Add(Contact contact)
        {
            lock (_lock)
            {
                _contacts.Add(contact);
            }
        }

        public void Update(Contact contact)
        {
            lock (_lock)
            {
                var index = _contacts.FindIndex(c => c.Id == contact.Id);
                if (index != -1)
                {
                    _contacts[index] = contact;
                }
            }
        }

        public void Delete(Guid id)
        {
            lock (_lock)
            {
                var contact = _contacts.FirstOrDefault(c => c.Id == id);
                if (contact is not null)
                {
                    _contacts.Remove(contact);
                }
            }
        }

        public Contact? GetById(Guid id)
        {
            lock (_lock)
            {
                return _contacts.FirstOrDefault(c => c.Id == id);
            }
        }

        public IEnumerable<Contact> GetByTag(string tag)
        {
            lock (_lock)
            {
                return _contacts
                    .Where(c => c.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        public IReadOnlyList<Contact> GetAll()
        {
            lock (_lock)
            {
                return _contacts.ToList();
            }
        }
    }

