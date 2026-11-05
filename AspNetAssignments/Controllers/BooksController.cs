using Microsoft.AspNetCore.Mvc;
using AspNetAssignments.DTOs.Book;
using AspNetAssignments.Models.Book;

namespace AspNetAssignments.Controllers;

/// <summary>
/// Контролер для операцій із каталогом книг
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private static readonly List<Book> _books = new()
    {
        new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Year = 2008 },
        new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Year = 1999 },
        new Book { Id = 3, Title = "Design Patterns", Author = "Erich Gamma", Year = 1994 }
    };

    private static int _nextId = 4;
    private static readonly object _lock = new();

    /// <summary>
    /// Отримує перелік усіх книг у каталозі
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Book>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_books.ToList());
        }
    }

    /// <summary>
    /// Знаходить книгу за її числовим ідентифікатором
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = "Книгу не знайдено." });
            }
            return Ok(book);
        }
    }

    /// <summary>
    /// Шукає книги за назвою та автором
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Search([FromQuery] string? title, [FromQuery] string? author = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest(new { message = "Обов'язковий параметр 'title' відсутній або порожній." });
        }

        lock (_lock)
        {
            var query = _books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(query.ToList());
        }
    }

    /// <summary>
    /// Додає нову книгу до каталогу
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateBookDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        lock (_lock)
        {
            var newBook = new Book
            {
                Id = _nextId++,
                Title = dto.Title.Trim(),
                Author = dto.Author.Trim(),
                Year = dto.Year
            };

            _books.Add(newBook);
            return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
        }
    }

    /// <summary>
    /// Оновлює дані наявної книги за ідентифікатором
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Update(int id, [FromBody] UpdateBookDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        lock (_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = "Книгу не знайдено." });
            }

            book.Title = dto.Title.Trim();
            book.Author = dto.Author.Trim();
            book.Year = dto.Year;

            return Ok(book);
        }
    }

    /// <summary>
    /// Видаляє книгу з каталогу
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        lock (_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound(new { message = "Книгу не знайдено." });
            }

            _books.Remove(book);
            return NoContent();
        }
    }
}
