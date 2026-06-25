using LexiFlow.BLL.Models.Deck;
using LexiFlow.BLL.Models.Review;
using LexiFlow.BLL.Models.ReviewSession;
using LexiFlow.BLL.Models.User;
using LexiFlow.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static LexiFlow.BLL.Exceptions.UserException;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/review-session")]
    public class ReviewSessionController : ControllerBase
    {
        private readonly IReviewSessionService _reviewSessionService;

        public ReviewSessionController(IReviewSessionService reviewSessionService)
        {
            _reviewSessionService = reviewSessionService;
        }

        [HttpPost("start")]
        public async Task<ResponseResult> StartSession(Guid deckId)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _reviewSessionService.StartSessionAsync(userId, deckId);

            return result;
        }

        [HttpPut("finish")]
        public async Task<ResponseResult> FinishSession(Guid sessionId)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _reviewSessionService.FinishSessionAsync(userId, sessionId);

            return result;
        }

        [HttpGet]
        public async Task<IEnumerable<ReviewSessionResponse>> GetAllSessions()
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _reviewSessionService.GetAllSessionsAsync(userId);

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ReviewSessionDetailResponse> GetSessionDetail(Guid id)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _reviewSessionService.GetSessionByIdAsync(userId, id);

            return result;
        }
    }
}
