using Microsoft.AspNetCore.Mvc;
using PhoneBook.Application.Dtos;
using PhoneBook.Application.Services;

namespace PhoneBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public IActionResult Create(CreateContactDto dto)
    {
        var result = _contactService.CreateContact(dto);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, UpdateContactDto dto)
    {
        var result = _contactService.UpdateContact(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var result = _contactService.DeleteContact(id);
        return Ok(result);
    }

    [HttpGet("tags/{tag}")]
    public IActionResult GetByTag(string tag)
    {
        var result = _contactService.GetContactsByTag(tag);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _contactService.GetAllContacts();
        return Ok(result);
    }
}
