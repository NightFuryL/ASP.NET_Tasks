using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AspNetAssignments.DTOs.Product;

public class CreateProductDTO
{
    [Required(ErrorMessage = "Назва товару є обов'язковою.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 1000000.0, ErrorMessage = "Ціна має бути більшою за нуль.")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 100000, ErrorMessage = "Кількість не може бути від'ємною.")]
    public int Quantity { get; set; }

    public List<IFormFile>? Images { get; set; }
}
