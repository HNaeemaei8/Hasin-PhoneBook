using FluentAssertions;
using Moq;
using PhoneBook.Application.Dtos;
using PhoneBook.Application.Services;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Repositories;
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
    public async Task AddContact_ValidDto_ShouldCallRepositorySave()
    {
        // Arrange
        var dto = new CreateContactDto(
            FirstName: "سینا",
            LastName: "احمدی",
            PhoneNumber: "09123456789",
            Tag: "کار"
        );

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Contact>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _contactService.CreateContactAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.FirstName.Should().Be("سینا");
        result.Value.LastName.Should().Be("احمدی");
        result.Value.PhoneNumber.Should().Be("09123456789");
        result.Value.Tag.Should().Be("کار");

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Once);
    }
}