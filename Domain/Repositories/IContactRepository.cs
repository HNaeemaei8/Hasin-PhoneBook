using PhoneBook.Domain.Entities;
namespace PhoneBook.Domain.Repositories;


    public interface IContactRepository
    {
        void Add(Contact contact);
        void Update(Contact contact);
        void Delete(Guid id);
        Contact? GetById(Guid id);
        IEnumerable<Contact> GetByTag(string tag);
        IReadOnlyList<Contact> GetAll();
    }

