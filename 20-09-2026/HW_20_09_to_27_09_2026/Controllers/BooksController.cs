using Microsoft.AspNetCore.Mvc;
using HW_20_09_to_27_09_2026.Models;

namespace HW_20_09_to_27_09_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    // GET /api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_books.ToList());
        }
    }

    // GET /api/books/{id:int}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }
            return Ok(book);
        }
    }

    // GET /api/books/search?title=...&author=...
    [HttpGet("search")]
    public IActionResult Search([FromQuery] string? title, [FromQuery] string? author = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("Обов'язковий параметр 'title' відсутній або порожній.");
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

    // POST /api/books
    [HttpPost]
    public IActionResult Create([FromBody] Book newBook)
    {
        if (newBook == null ||
            string.IsNullOrWhiteSpace(newBook.Title) ||
            string.IsNullOrWhiteSpace(newBook.Author) ||
            newBook.Year < 1800)
        {
            return BadRequest("Поля Title та Author є обов'язковими, а Year має бути не менше 1800.");
        }

        lock (_lock)
        {
            newBook.Id = _nextId++;
            _books.Add(newBook);
            return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
        }
    }

    // PUT /api/books/{id:int}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Book updatedBook)
    {
        if (updatedBook == null ||
            string.IsNullOrWhiteSpace(updatedBook.Title) ||
            string.IsNullOrWhiteSpace(updatedBook.Author) ||
            updatedBook.Year < 1800)
        {
            return BadRequest("Некоректні дані: поля Title та Author обов'язкові, а Year >= 1800.");
        }

        lock (_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Year = updatedBook.Year;

            return Ok(book);
        }
    }

    // DELETE /api/books/{id:int}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        lock (_lock)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            _books.Remove(book);
            return NoContent();
        }
    }
}
