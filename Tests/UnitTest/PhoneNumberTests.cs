using FluentAssertions;
using PhoneBook.Domain.ValueObjects;
using Xunit;

namespace PhoneBook.Tests.Domain;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("09121234567")]
    [InlineData("+989121234567")]
    public void Create_WithValidNumber_ShouldCreateInstance(string validNumber)
    {
         var phoneNumber = PhoneNumber.Create(validNumber);

        // Assert
        phoneNumber.Value.Should().Be(validNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")]
    [InlineData("abc12345678")]
    public void Create_WithInvalidNumber_ShouldThrowArgumentException(string invalidNumber)
    {
        Action act = () => PhoneNumber.Create(invalidNumber);

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage("*معتبر نیست*");
    }
}