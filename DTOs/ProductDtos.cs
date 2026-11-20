using System.ComponentModel.DataAnnotations;

namespace VueStoreApi.DTOs;

public record ProductResponse(
    int Id, string Name, string Description, decimal Price,
    string ImageUrl, string Category, int Stock, DateTime CreatedAtUtc);

public class ProductRequest
{
    [Required, StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Price { get; set; }

    [Url, StringLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
