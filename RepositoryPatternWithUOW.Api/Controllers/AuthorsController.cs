using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternWithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        //private readonly IBaseRepository<Author> _authorRepository;
        private readonly IUOW _uow;
        public AuthorsController(IUOW uOW) //IBaseRepository<Author> authorRepository
        {
            //_authorRepository = authorRepository;
            _uow = uOW;
        }


        [HttpGet("GetAuthorById")]
        public IActionResult GetAuthorById(int id)
        {
            //return Ok(_authorRepository.GetById(id));
            return Ok(_uow.Authors.GetById(id));
        }

        [HttpGet("GetAuthorByIdAsync")]
        public async Task<IActionResult> GetAuthorByIdAsync(int id)
        {
            return Ok(await _uow.Authors.GetByIdAsync(id));
        }

        [HttpGet("GetAllAuthorsAsync")]
        public async Task<IActionResult> GetAllAuthorsAsync()
        {
            return Ok(await _uow.Authors.GetAllAsync());
        }

        [HttpGet("GetAuthorsByCriteriaAsync")]
        public async Task<IActionResult> GetAuthorsByCriteriaAsync(int id)
        {
            return Ok(await _uow.Authors.FindAsync(x => x.Id == id));
        }
    }
}
