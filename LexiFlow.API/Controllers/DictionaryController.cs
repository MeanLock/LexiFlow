using LexiFlow.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/dictionary")]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(
            IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        [HttpGet("word")]
        public async Task<IActionResult> GetWordDefinition(
            [FromQuery] string word,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return BadRequest("Word is required.");
            }

            var result =
                await _dictionaryService.GetWordDefinitionAsync(
                    word,
                    cancellationToken);
            Console.WriteLine($"Result: {result}");
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
