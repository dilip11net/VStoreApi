using System.ComponentModel.DataAnnotations;

namespace VueStoreApi.DTOs;

public class RegisterRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6), StringLength(100)]
    public string Password { get; set; } = string.Empty;
    [Required, MinLength(6), StringLength(100)]

    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public record AuthResponse(int UserId, string Name, string Email, string Token, DateTime ExpiresAtUtc);
