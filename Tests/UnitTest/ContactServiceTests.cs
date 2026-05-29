using FluentAssertions;
using Moq;
using PhoneBook.Application.Dtos;
using PhoneBook.Application.Services;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Errors;
using PhoneBook.Domain.Repositories;
using PhoneBook.Domain.ValueObjects;
using Xunit;

namespace PhoneBook.Tests.Application;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _repositoryMock;
    private readonly ContactService _contactService;

    public ContactServiceTests()
    {
        _repositoryMock = new Mock<IContactRepository>();
        _contactService = new ContactService(_repositoryMock.Object);
    }

    [Fact]
    public void CreateContact_WithValidDto_ShouldReturnSuccessAndCallRepositoryAdd()
    {
        // Arrange
        var dto = new CreateContactDto(
            FirstName: "سینا",
            LastName: "احمدی",
            PhoneNumber: "09123456789",
            Tag: "کار"
        );

        _repositoryMock.Setup(r => r.Add(It.IsAny<Contact>()));

        // Act
        var result = _contactService.CreateContact(dto); 

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.FirstName.Should().Be("سینا");
        result.Value.LastName.Should().Be("احمدی");
        result.Value.PhoneNumber.Should().Be("09123456789");
        result.Value.Tag.Should().Be("کار");

        _repositoryMock.Verify(r => r.Add(It.Is<Contact>(c =>
            c.FirstName == "سینا" &&
            c.LastName == "احمدی" &&
            c.PhoneNumber.Value == "09123456789" &&
            c.Tag == "کار"
        )), Times.Once);
    }

    [Fact]
    public void CreateContact_WithInvalidPhoneNumber_ShouldReturnFailureAndNotCallRepository()
    {
        // Arrange
        var dto = new CreateContactDto("سینا", "احمدی", "123", "کار");

        // Act
        var result = _contactService.CreateContact(dto);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(DomainErrorCode.InvalidPhoneNumber);

        _repositoryMock.Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
    }

    [Fact]
    public void UpdateContact_WhenContactDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new UpdateContactDto("علی", "رضایی", "09121234567", "دوست");

        _repositoryMock.Setup(r => r.GetById(id)).Returns((Contact?)null);

        // Act
        var result = _contactService.UpdateContact(id, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
        _repositoryMock.Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
    }

    [Fact]
    public void DeleteContact_WhenContactDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetById(id)).Returns((Contact?)null);

        // Act
        var result = _contactService.DeleteContact(id);

        // Assert
        result.IsFailure.Should().BeTrue();
        _repositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void UpdateContact_WithValidData_ShouldUpdateContactAndReturnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        var phoneResult = PhoneNumber.Create("09121234567");
        var contactResult = Contact.Create("علی", "رضایی", phoneResult.Value, "قدیمی");
        var contact = contactResult.Value;

        var dto = new UpdateContactDto("علیرضا", "احمدی", "09301112233", "جدید");

        _repositoryMock.Setup(r => r.GetById(id)).Returns(contact);

        // Act
        var result = _contactService.UpdateContact(id, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        contact.FirstName.Should().Be("علیرضا");
        contact.LastName.Should().Be("احمدی");
        contact.PhoneNumber.Value.Should().Be("09301112233");
        contact.Tag.Should().Be("جدید");

        _repositoryMock.Verify(r => r.Update(contact), Times.Once);
    }

    [Fact]
    public void DeleteContact_WhenContactExists_ShouldDeleteAndReturnSuccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        var phoneResult = PhoneNumber.Create("09121234567");
        var contactResult = Contact.Create("علی", "رضایی", phoneResult.Value, "دوست");
        var contact = contactResult.Value;

        _repositoryMock.Setup(r => r.GetById(id)).Returns(contact);

        // Act
        var result = _contactService.DeleteContact(id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.Delete(id), Times.Once);
    }

    [Fact]
    public void GetAllContacts_WhenRepositoryHasData_ShouldReturnMappedDtos()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("09121234567").Value;
        var phone2 = PhoneNumber.Create("09301112233").Value;

        var contact1 = Contact.Create("علی", "رضایی", phone1, "دوست").Value;
        var contact2 = Contact.Create("سارا", "محمدی", phone2, "کار").Value;

        _repositoryMock.Setup(r => r.GetAll()).Returns(new[] { contact1, contact2 });

        // Act
        var result = _contactService.GetAllContacts();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(c => c.FirstName == "علی");
        result.Value.Should().Contain(c => c.FirstName == "سارا");
    }

    [Fact]
    public void GetContactsByTag_WithExistingTag_ShouldReturnFilteredContacts()
    {
        // Arrange
        var phone = PhoneNumber.Create("09121234567").Value;
        var contact = Contact.Create("علی", "رضایی", phone, "کار").Value;

        _repositoryMock.Setup(r => r.GetByTag("کار")).Returns(new[] { contact });

        // Act
        var result = _contactService.GetContactsByTag("کار");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().Tag.Should().Be("کار");
    }

    [Fact]
    public void UpdateContact_WithInvalidPhoneNumber_ShouldReturnFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var phone = PhoneNumber.Create("09121234567").Value;
        var contact = Contact.Create("علی", "رضایی", phone, "دوست").Value;

        var dto = new UpdateContactDto("علیرضا", "احمدی", "123", "کار");

        _repositoryMock.Setup(r => r.GetById(id)).Returns(contact);

        // Act
        var result = _contactService.UpdateContact(id, dto);

        // Assert
        result.IsFailure.Should().BeTrue();
        _repositoryMock.Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
    }
}