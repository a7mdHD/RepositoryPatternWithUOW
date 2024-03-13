using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Consts;
using RepositoryPatternWithUOW.Core.Dtos;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternWithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //private readonly IBaseRepository<Book> _bookRepository;
        private readonly IUOW _uOW;
        public BooksController(IUOW uOW) //IBaseRepository<Book> bookRepository
        {
            //_bookRepository = bookRepository;
            _uOW = uOW;
        }

        [HttpGet("GetBookById")]
        public IActionResult GetBookById(int id)
        {
            return Ok(_uOW.Books.GetById(id));
        }

        [HttpGet("GetBookByCriteraAsync")]
        public async Task<IActionResult> GetBookByCriteraAsync(int id)
        {
            return Ok(await _uOW.Books.FindAsync(x => x.Id == id, new[] { "Author" }));
        }

        [HttpGet("GetAllBooksByCriteraAsync")]
        public async Task<IActionResult> GetAllBooksByCriteraAsync()
        {
            return Ok(await _uOW.Books.FindAllAsync(b => b.Title.Contains("Hello"), new[] { "Author" }));
        }

        [HttpGet("GetAllBooksOrderdByAsync")]
        public async Task<IActionResult> GetAllBooksOrderdByAsync()
        {
            return Ok(await _uOW.Books.FindAllAsync(b => b.Title.Contains("Hello"), null, null, b => b.Id, OrderBy.Descending));
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook([FromBody] BookDto book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _uOW.Books.Add(new Book { Title = book.Title, AuthorId = book.AuthorId });
            _uOW.Save();
            return Ok(book);    
        }

        [HttpPost("AddBooks")]
        public async Task<IActionResult> AddBooks([FromBody] List<BookDto> books)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var bookList = new List<Book>();
            foreach (var book in books)
                bookList.Add( new Book { Title = book.Title, AuthorId=book.AuthorId });

            var result = await _uOW.Books.AddRange(bookList);
            _uOW.Save();
            return Ok(books);      
        }
    }
}
