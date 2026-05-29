using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PhoneBook.Application.Dtos;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace PhoneBook.Tests.Integration;

public class ContactApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContactApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task CreateContact_WithValidData_ShouldReturnCreatedStatusCode()
    {
        // Arrange
        var payload = new CreateContactDto
        (
            FirstName: "تست",
            LastName: "نهایی",
            PhoneNumber: "09121112233",
            Tag: "همکار"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacts", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<ContactDto>();
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("تست");

    }

    [Fact]
    public async Task CreateContact_WithInvalidPhone_ShouldReturnBadRequest()
    {
        // Arrange
        var payload = new CreateContactDto
        (
            FirstName: "نامعتبر",
            LastName: "تست",
            PhoneNumber: "123", 
            Tag: "عمومی"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacts", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}