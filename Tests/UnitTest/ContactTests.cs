using FluentAssertions;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.ValueObjects;
using Xunit;

namespace PhoneBook.Tests.Domain;

public class ContactTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateContactCorrectly()
    {
        // Arrange
        var firstName = "امیرحسین";
        var lastName = "رضایی";
        var phoneStr = "09121234567";
        var phoneNumberResult = PhoneNumber.Create(phoneStr);
        var tag = "عمومی";

        // Assert precondition
        phoneNumberResult.IsSuccess.Should().BeTrue();

        // Act
        var contact = new Contact(firstName, lastName, phoneNumberResult.Value, tag);

        // Assert
        contact.Id.Should().NotBeEmpty();
        contact.FirstName.Should().Be(firstName);
        contact.LastName.Should().Be(lastName);
        contact.PhoneNumber.Value.Should().Be(phoneStr);
        contact.Tag.Should().Be(tag);
    }

    [Fact]
    public void Update_WithNewData_ShouldModifyContactProperties()
    {
        // Arrange
        var phoneNumberResult = PhoneNumber.Create("09121234567");
        phoneNumberResult.IsSuccess.Should().BeTrue();

        var contact = new Contact("علی", "رضایی", phoneNumberResult.Value, "قدیمی");

        var newFirstName = "علیرضا";
        var newLastName = "احمدی";
        var newPhoneResult = PhoneNumber.Create("09301112233");
        var newTag = "جدید";

        newPhoneResult.IsSuccess.Should().BeTrue();

        // Act
        contact.Update(newFirstName, newLastName, newPhoneResult.Value, newTag);

        // Assert
        contact.FirstName.Should().Be(newFirstName);
        contact.LastName.Should().Be(newLastName);
        contact.PhoneNumber.Value.Should().Be("09301112233");
        contact.Tag.Should().Be(newTag);
    }

    [Fact]
    public void PhoneNumber_CreateWithInvalidFormat_ShouldReturnFailureResult()
    {
        // Arrange
        var invalidPhone = "123";

        // Act
        var result = PhoneNumber.Create(invalidPhone);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}