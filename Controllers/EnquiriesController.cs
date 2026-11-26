using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueStoreApi.Data;
using VueStoreApi.DTOs;
using VueStoreApi.Models;

namespace VueStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnquiriesController(StoreDbContext db) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<EnquiryResponse>> Create(EnquiryRequest request)
    {
        var enquiry = new Enquiry
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Message = request.Message.Trim()
        };

        db.Enquiries.Add(enquiry);
        await db.SaveChangesAsync();

        var response = new EnquiryResponse(enquiry.Id, enquiry.Name, enquiry.Email,
            enquiry.Message, enquiry.CreatedAtUtc);
        return CreatedAtAction(nameof(GetById), new { id = enquiry.Id }, response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EnquiryResponse>> GetById(int id)
    {
        var e = await db.Enquiries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() :
            Ok(new EnquiryResponse(e.Id, e.Name, e.Email, e.Message, e.CreatedAtUtc));
    }
}
