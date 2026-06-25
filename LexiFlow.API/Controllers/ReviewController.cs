using LexiFlow.BLL.Models.Deck;
using LexiFlow.BLL.Models.Review;
using LexiFlow.BLL.Models.User;
using LexiFlow.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static LexiFlow.BLL.Exceptions.UserException;

namespace LexiFlow.API.Controllers
{

    [ApiController]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("submit-card-review")]
        public async Task<ResponseResult> SubmitReview([FromBody] SubmitReviewRequest request)
        {
            var userClaims = HttpContext.User;
            var userIdClaim = userClaims.FindFirst("UserId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new ForbiddenException();
            }

            var result = await _reviewService.SubmitReviewAsync(userId, request);

            return result;
        }
    }

}
