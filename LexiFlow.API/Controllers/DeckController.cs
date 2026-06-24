using LexiFlow.BLL.Models.Deck;
using LexiFlow.BLL.Models.User;
using LexiFlow.BLL.Services;
using LexiFlow.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static LexiFlow.BLL.Exceptions.UserException;

namespace LexiFlow.API.Controllers
{
    [Route("api/decks")]
    [ApiController]
    [Authorize(Policy = "LearnerOnly")]
    public class DeckController : ControllerBase
    {
        private readonly IDeckService _deckService;

        public DeckController(IDeckService deckService)
        {
            _deckService = deckService;
        }

        [HttpPost]
        public async Task<ResponseResult> CreateDeck([FromBody] CreateDeckRequest request)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _deckService.AddDeckAsync(userId, request);

            return result;
        }

        [HttpGet]
        public async Task<IEnumerable<DeckResponse>> GetAllDecks()
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var decks = await _deckService.GetAllDecksAsync(userId);
            return decks;
        }

        [HttpGet("{id}")]
        public async Task<DeckResponse> GetDeck(Guid id)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var decks = await _deckService.GetDeckByIdAsync(userId, id);
            return decks;
        }

        [HttpPut("{id}")]
        public async Task<ResponseResult> UpdateDeck(Guid id, [FromBody] UpdateDeckRequest request)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _deckService.UpdateDeckAsync(userId, id, request);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ResponseResult> DeleteDeck(Guid id)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _deckService.DeleteDeckAsync(userId, id);
            return result;
        }
    }
}
