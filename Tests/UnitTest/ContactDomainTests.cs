using FluentAssertions;
using PhoneBook.Domain.Entities;
using PhoneBook.Domain.Errors;
using PhoneBook.Domain.ValueObjects;
using Xunit;

namespace PhoneBook.Tests.Domain;

public class ContactDomainTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateContactCorrectly()
    {
        // Arrange
        var firstName = "امیرحسین";
        var lastName = "رضایی";
        var phoneStr = "09121234567";
        var phoneNumberResult = PhoneNumber.Create(phoneStr);
        var tag = "عمومی";

        // Assert 
        phoneNumberResult.IsSuccess.Should().BeTrue();

        var result = Contact.Create(firstName, lastName, phoneNumberResult.Value, tag);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
        result.Value.PhoneNumber.Value.Should().Be(phoneStr);
        result.Value.Tag.Should().Be(tag);
    }

    [Fact]
    public void Update_WithNewData_ShouldModifyContactProperties()
    {
        //  ARRANGE 
        var phoneResult = PhoneNumber.Create("09121234567");
        var contact = Contact.Create("علی", "رضایی", phoneResult.Value, "قدیمی").Value;

        var newFirstName = "علیرضا";
        var newLastName = "احمدی";
        var newPhoneResult = PhoneNumber.Create("09301112233");
        var newTag = "جدید";

        newPhoneResult.IsSuccess.Should().BeTrue();

        // ACT 
        contact.Update(newFirstName, newLastName, newPhoneResult.Value, newTag);

        // ASSERT 
        contact.FirstName.Should().Be(newFirstName);
        contact.LastName.Should().Be(newLastName);
        contact.PhoneNumber.Value.Should().Be("09301112233");
        contact.Tag.Should().Be(newTag);
    }
    [Fact]
    public void Create_WithEmptyFirstName_ShouldReturnFailure()
    {
        // Arrange
        var phone = PhoneNumber.Create("09121234567").Value;

        // Act
        var result = Contact.Create("", "رضایی", phone, "دوست");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(DomainErrorCode.NameIsRequired);
    }

}