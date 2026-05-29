using FluentAssertions;
using PhoneBook.Domain.Common;
using PhoneBook.Domain.Errors;
using PhoneBook.Domain.ValueObjects;
using Xunit;

namespace PhoneBook.Tests.Domain;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("09121234567")]
    [InlineData("+989121234567")]
    public void Create_WithValidNumber_ShouldReturnSuccessResult(string validNumber)
    {
        var result = PhoneNumber.Create(validNumber);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull(); 
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")]
    [InlineData("abc12345678")]
    public void Create_WithInvalidNumber_ShouldReturnFailureResult(string invalidNumber)
    {
        var result = PhoneNumber.Create(invalidNumber);

        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(DomainErrorCode.InvalidPhoneNumber);
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

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("0912")]
    [InlineData("abcd")]
    public void Create_WithInvalidPhoneNumber_ShouldReturnFailure(string invalidPhone)
    {
        // Act
        var result = PhoneNumber.Create(invalidPhone);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorCode.Should().Be(DomainErrorCode.InvalidPhoneNumber);
    }
}