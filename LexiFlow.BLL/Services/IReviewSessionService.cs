using LexiFlow.BLL.Models.ReviewSession;
using LexiFlow.BLL.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.BLL.Services
{
    public interface IReviewSessionService
    {
        Task<ResponseResult> StartSessionAsync(Guid userId, Guid deckId);

        Task<ResponseResult> FinishSessionAsync(Guid userId, Guid sessionId);

        Task<IEnumerable<ReviewSessionResponse>> GetAllSessionsAsync(Guid userId);

        Task<ReviewSessionDetailResponse> GetSessionByIdAsync(Guid userId, Guid sessionId);
    }
}
