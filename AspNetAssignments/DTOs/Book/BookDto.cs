using System.ComponentModel.DataAnnotations;

namespace AspNetAssignments.DTOs.Book;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
}

public class CreateBookDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Range(1800, 2100, ErrorMessage = "Рік видання має бути між 1800 та 2100.")]
    public int Year { get; set; }
}

public class UpdateBookDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Range(1800, 2100, ErrorMessage = "Рік видання має бути між 1800 та 2100.")]
    public int Year { get; set; }
}
