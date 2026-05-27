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

    public async Task<Result<ContactDto>> CreateContactAsync(CreateContactDto dto)
    {
        var phoneResult = PhoneNumber.Create(dto.PhoneNumber);
        if (phoneResult.IsFailure)
            return Result<ContactDto>.Failure(phoneResult.ErrorCode);

        var contact = new Contact(dto.FirstName, dto.LastName, phoneResult.Value, dto.Tag);

        await _repository.AddAsync(contact);

        return Result<ContactDto>.Success(new ContactDto(
            contact.Id, contact.FirstName, contact.LastName, contact.PhoneNumber.Value, contact.Tag));
    }

    public async Task<Result> UpdateContactAsync(Guid id, UpdateContactDto dto)
    {
        var contact = await _repository.GetByIdAsync(id);
        if (contact == null)
            return Result.Failure(DomainErrorCode.ContactNotFound);

        var phoneResult = PhoneNumber.Create(dto.PhoneNumber);
        if (phoneResult.IsFailure)
            return Result.Failure(phoneResult.ErrorCode);

        contact.Update(dto.FirstName, dto.LastName, phoneResult.Value, dto.Tag);

        await _repository.UpdateAsync(contact);
        return Result.Success();
    }

    public async Task<Result> DeleteContactAsync(Guid id)
    {
        var contact = await _repository.GetByIdAsync(id);
        if (contact == null)
            return Result.Failure(DomainErrorCode.ContactNotFound);

        await _repository.DeleteAsync(id);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<ContactDto>>> GetContactsByTagAsync(string tag)
    {
        var contacts = await _repository.GetByTagAsync(tag);
        var result = contacts.Select(c => new ContactDto(c.Id, c.FirstName, c.LastName, c.PhoneNumber.Value, c.Tag));
        return Result<IEnumerable<ContactDto>>.Success(result);
    }

    public async Task<Result<IEnumerable<ContactDto>>> GetAllContactsAsync()
    {
        var contacts = await _repository.GetAllAsync();
        var result = contacts.Select(c => new ContactDto(c.Id, c.FirstName, c.LastName, c.PhoneNumber.Value, c.Tag));
        return Result<IEnumerable<ContactDto>>.Success(result);
    }
}
