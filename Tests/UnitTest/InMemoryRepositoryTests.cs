using FluentAssertions;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.ValueObjects;
using PhoneBook.Infrastructure.Persistence;
using Xunit;

namespace PhoneBook.Tests.Infrastructure;

    public class InMemoryRepositoryTests
    {
    
    [Fact]
    public async Task AddAsync_ShouldAddContactToInMemoryList()
    {
        // Arrange
        var repository = new InMemoryContactRepository();

        var phoneResult = PhoneNumber.Create("09125555555");

        phoneResult.IsSuccess.Should().BeTrue();

        var createResult = Contact.Create("نیما", "رضایی", phoneResult.Value, "عمومی");
        createResult.IsSuccess.Should().BeTrue();

        var contact = createResult.Value;

        // Act
        await repository.AddAsync(contact);
        var result = await repository.GetByIdAsync(contact.Id);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("نیما");
        result.LastName.Should().Be("رضایی");
        result.PhoneNumber.Value.Should().Be("09125555555");
    }
}