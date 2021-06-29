using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;

namespace TestOData.Api.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private const string ReceivedRequest = "Received request.";

        private readonly ILogger<BooksController> _logger;
        private readonly IBooksService _bookService;

        public BooksController(
            ILogger<BooksController> logger,
            IBooksService weatherService
            )
        {
            _logger = logger;
            _bookService = weatherService;
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Book), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var bookData = await _bookService.GetBooks().ConfigureAwait(false);

                return new OkObjectResult(bookData);
            }
            catch (NotFoundException ex)
            {
                _logger.LogDebug(ex.Message);

                return NotFound();
            }
        }
        [HttpGet("{key:int}")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Book), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int key)
        {
            try
            {
                var bookData = await _bookService.GetBook(key).ConfigureAwait(false);

                return new OkObjectResult(bookData);
            }
            catch (NotFoundException ex)
            {
                _logger.LogDebug(ex.Message);

                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Book), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            book = await _bookService.CreateBook(book);
            return new ObjectResult(book);
        }

    }
}
