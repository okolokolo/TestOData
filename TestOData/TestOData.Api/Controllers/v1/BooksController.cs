using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSM.Core.Logging.Extensions.Adapters;
using RSM.Core.Logging.Shared;
using System.Threading.Tasks;
using TestOData.Interfaces.Service;
using TestOData.Model;
using TestOData.Service.Exceptions;

namespace TestOData.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BooksController : ControllerBase
    {
        private const string ReceivedRequest = "Received request.";

        private readonly ILoggerAdapter<BooksController> _logger;
        private readonly IBooksService _bookService;

        public BooksController(
            ILoggerAdapter<BooksController> logger,
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
            _logger.LogDebug(ReceivedRequest, new LogItem<BooksController>());

            try
            {
                var bookData = await _bookService.GetBooks().ConfigureAwait(false);

                return new OkObjectResult(bookData);
            }
            catch (NotFoundException ex)
            {
                _logger.LogDebug(ex.Message, new LogItem<BooksController>());

                return NotFound();
            }
        }
        [HttpGet("{key:int}")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Book), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int key)
        {
            _logger.LogDebug(ReceivedRequest, new LogItem<BooksController>());

            try
            {
                var bookData = await _bookService.GetBook(key).ConfigureAwait(false);

                return new OkObjectResult(bookData);
            }
            catch (NotFoundException ex)
            {
                _logger.LogDebug(ex.Message, new LogItem<BooksController>());

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
