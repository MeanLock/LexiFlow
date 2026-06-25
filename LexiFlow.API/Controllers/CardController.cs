using LexiFlow.BLL.Models.Card;
using LexiFlow.BLL.Models.Deck;
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
        public async Task<List<CardResponse>> GetDueCards(Guid deckId)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _cardService.GetDueCardsAsync(userId, deckId);
            return result;
        }
    }
}
