using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Domain.Entities;

public class Contact
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } 
    public string Tag { get; private set; } 

    public Contact(string firstName, string lastName, PhoneNumber phoneNumber, string tag)
    {
        Id = Guid.NewGuid();
        ValidateAndSet(firstName, lastName, phoneNumber, tag);
    }

    public void Update(string firstName, string lastName, PhoneNumber phoneNumber, string tag)
    {
        ValidateAndSet(firstName, lastName, phoneNumber, tag);
    }

    private void ValidateAndSet(string firstName, string lastName, PhoneNumber phoneNumber, string tag)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("نام الزامی است.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("نام خانوادگی الزامی است.");

        if (phoneNumber == null)
            throw new ArgumentNullException(nameof(phoneNumber), "شماره تلفن الزامی است.");

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Tag = string.IsNullOrWhiteSpace(tag) ? "General" : tag; // اگر تگ خالی بود مقدار پیش‌فرض می‌گیرد
    }
}