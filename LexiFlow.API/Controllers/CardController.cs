using LexiFlow.BLL.Models.Card;
using LexiFlow.BLL.Models.Card;
using LexiFlow.BLL.Models.User;
using LexiFlow.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static LexiFlow.BLL.Exceptions.UserException;

namespace LexiFlow.API.Controllers
{
    [Route("api/cards")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet("due-cards")]
        public async Task<List<CardResponse>> GetDueCards(Guid cardId)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _cardService.GetDueCardsAsync(userId, cardId);
            return result;
        }

        [HttpPost]
        public async Task<ResponseResult> CreateCard([FromBody] CreateCardRequest request)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _cardService.CreateCardAsync(userId, request);

            return result;
        }

        [HttpGet]
        public async Task<IEnumerable<CardResponse>> GetCardByDeckId(Guid deckId)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var cards = await _cardService.GetCardsByDeckAsync(userId, deckId);
            return cards;
        }

        [HttpGet("{id}")]
        public async Task<CardResponse> GetCardById(Guid id)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var cards = await _cardService.GetCardByIdAsync(userId, id);
            return cards;
        }

        [HttpPut("{id}")]
        public async Task<ResponseResult> UpdateCard(Guid id, [FromBody] CreateCardRequest request)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _cardService.UpdateCardAsync(userId, id, request);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ResponseResult> DeleteCard(Guid id)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _cardService.DeleteCardAsync(userId, id);
            return result;
        }
    }
}
