using System.ComponentModel.DataAnnotations;

namespace ExtraAspNetAssignments.Models.Book;

public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    public int Year { get; set; }

    public bool IsDeleted { get; set; } = false;
}
