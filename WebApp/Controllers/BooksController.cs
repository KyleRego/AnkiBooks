using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookRepository bookRepository)
{
    private readonly IBookRepository _bookRepository = bookRepository;

    // GET: api/BasicNotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> PublicIndex()
    {
        return await _bookRepository.GetPublicBooksAsync();
    }

}