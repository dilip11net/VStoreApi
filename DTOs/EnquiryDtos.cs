using System.ComponentModel.DataAnnotations;

namespace VueStoreApi.DTOs;

public class EnquiryRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(4000, MinimumLength = 5)]
    public string Message { get; set; } = string.Empty;
}

public record EnquiryResponse(int Id, string Name, string Email, string Message, DateTime CreatedAtUtc);
