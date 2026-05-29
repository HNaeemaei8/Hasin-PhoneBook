using PhoneBook.Application.Dtos;
using PhoneBook.Domain.Common;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Errors;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;

namespace PhoneBook.Application.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }

    public Result<ContactDto> CreateContact(CreateContactDto dto)
    {
        var phoneResult = PhoneNumber.Create(dto.PhoneNumber);
        if (phoneResult.IsFailure)
            return Result<ContactDto>.Failure(phoneResult.ErrorCode);

        var contactResult = Contact.Create(dto.FirstName, dto.LastName, phoneResult.Value, dto.Tag);
        if (contactResult.IsFailure)
            return Result<ContactDto>.Failure(contactResult.ErrorCode);

        var contact = contactResult.Value;

        _repository.Add(contact);

        return Result<ContactDto>.Success(new ContactDto(
            contact.Id, contact.FirstName, contact.LastName, contact.PhoneNumber.Value, contact.Tag));
    }

    public Result UpdateContact(Guid id, UpdateContactDto dto)
    {
        var contact = _repository.GetById(id);
        if (contact == null)
            return Result.Failure(DomainErrorCode.ContactNotFound);

        var phoneResult = PhoneNumber.Create(dto.PhoneNumber);
        if (phoneResult.IsFailure)
            return Result.Failure(phoneResult.ErrorCode);

        contact.Update(dto.FirstName, dto.LastName, phoneResult.Value, dto.Tag);

        _repository.Update(contact);
        return Result.Success();
    }

    public Result DeleteContact(Guid id)
    {
        var contact = _repository.GetById(id);
        if (contact == null)
            return Result.Failure(DomainErrorCode.ContactNotFound);

        _repository.Delete(id);
        return Result.Success();
    }

    public Result<IEnumerable<ContactDto>> GetContactsByTag(string tag)
    {
        var contacts = _repository.GetByTag(tag);
        var result = contacts.Select(c => new ContactDto(
            c.Id, c.FirstName, c.LastName, c.PhoneNumber.Value, c.Tag));

        return Result<IEnumerable<ContactDto>>.Success(result);
    }

    public Result<IEnumerable<ContactDto>> GetAllContacts()
    {
        var contacts = _repository.GetAll();
        var result = contacts.Select(c => new ContactDto(
            c.Id, c.FirstName, c.LastName, c.PhoneNumber.Value, c.Tag));

        return Result<IEnumerable<ContactDto>>.Success(result);
    }
}