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
    public async Task<IActionResult> Create(CreateContactDto dto)
    {
        // ما کل Result را برمی‌گردانیم، Filter خودش Value را استخراج می‌کند
        return Ok(await _contactService.CreateContactAsync(dto));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateContactDto dto)
    {
        // نتیجه (موفقیت یا شکست) توسط Filter مدیریت می‌شود
        return Ok(await _contactService.UpdateContactAsync(id, dto));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _contactService.DeleteContactAsync(id));
    }

    [HttpGet("tags/{tag}")]
    public async Task<IActionResult> GetByTag(string tag)
    {
        return Ok(await _contactService.GetContactsByTagAsync(tag));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _contactService.GetAllContactsAsync());
    }
}
